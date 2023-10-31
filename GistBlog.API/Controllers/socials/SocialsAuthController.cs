using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs.socials;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers.socials;

[ApiController]
[Route("api/vi/")]
public class SocialsAuthController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public SocialsAuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDto googleAuth)
    {
        return await _googleAuthService.ExternalLogin(googleAuth);
    }
}
