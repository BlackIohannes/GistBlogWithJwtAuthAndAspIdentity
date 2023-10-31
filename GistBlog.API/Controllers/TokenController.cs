using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class TokenController : ControllerBase
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public TokenController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> Refresh(RefreshTokenRequestDto tokenRequestDto)
    {
        if (tokenRequestDto is null)
            return BadRequest("Invalid client request");

        var accessToken = tokenRequestDto.AccessToken;
        var refreshToken = tokenRequestDto.RefreshToken;
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity!.Name;
        var user = await _context.TokenInfos!.SingleOrDefaultAsync(x => x.Username == username);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GetToken(principal.Claims);
        var newRefreshToken = _tokenService.GetRefreshToken();
        user.RefreshToken = newRefreshToken;

        await _context.SaveChangesAsync();

        return Ok(new RefreshTokenRequestDto
        {
            AccessToken = newAccessToken.TokenString,
            RefreshToken = newRefreshToken
        });
    }

    // Revoke is used for revoking token entry from the db
    [HttpPost("revoke-token")]
    public async Task<ActionResult> Revoke()
    {
        // Check if the "Authorization" header is present and not empty
        if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) ||
            string.IsNullOrEmpty(authorizationHeader))
            return BadRequest("Authorization header is missing or empty");

        var principal = _tokenService.GetPrincipalFromExpiredToken(authorizationHeader);

        // Extract the username from the principal
        var username = principal.Identity.Name;

        // Find the user in the database
        var user = await _context.TokenInfos.SingleOrDefaultAsync(x => x.Username == username);
        if (user == null) return BadRequest("Invalid client request");

        // Revoke the refresh token
        user.RefreshToken = null;
        user.RefreshTokenExpiryDate = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok();
    }
}
