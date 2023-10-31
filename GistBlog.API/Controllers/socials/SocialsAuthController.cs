using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs.socials;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers.socials;

[ApiController]
[Route("api/vi/")]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public GoogleAuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("external-login")]
    public async Task<IActionResult> ExternalLogin([FromBody] GoogleAuthDto googleAuth)
    {
        return await _googleAuthService.ExternalLogin(googleAuth);
    }
}
