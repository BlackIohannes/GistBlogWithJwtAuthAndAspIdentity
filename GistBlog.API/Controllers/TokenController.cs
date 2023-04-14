using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TokenController : ControllerBase
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public TokenController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult> Refresh(RefreshTokenRequestDto tokenRequestDto)
    {
        if (tokenRequestDto is null)
            return BadRequest("Invalid client request");

        var accessToken = tokenRequestDto.AccessToken;
        var refreshToken = tokenRequestDto.RefreshToken;
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name;
        var user = await _context.TokenInfos.SingleOrDefaultAsync(x => x.Username == username);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GetToken(principal.Claims);
        var newRefreshToken = _tokenService.GetRefreshToken();
        user.RefreshToken = newRefreshToken;

        await _context.SaveChangesAsync();

        return Ok(new RefreshTokenRequestDto()
        {
            AccessToken = newAccessToken.TokenString,
            RefreshToken = newRefreshToken
        });
    }

    // Revoke is used for revoking token entry from the db
    [HttpPost("RevokeToken")]
    public async Task<ActionResult> Revoke()
    {
        try
        {
            var username = User.Identity.Name;
            var user = await _context.TokenInfos.SingleOrDefaultAsync(x => x.Username == username);
            if (user is null)
                return BadRequest();

            user.RefreshToken = null;
            await _context.SaveChangesAsync();

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
