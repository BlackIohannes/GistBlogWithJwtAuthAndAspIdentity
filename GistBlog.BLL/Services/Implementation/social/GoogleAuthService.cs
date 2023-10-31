using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs.socials;
using GistBlog.DAL.Entities.Models.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.BLL.Services.Implementation.social;

public class ExternalAuthService : IExternalAuthService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public ExternalAuthService(ITokenService tokenService, UserManager<AppUser> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<IActionResult> ExternalLogin(GoogleAuthDto googleAuth)
    {
        try
        {
            var payload = await _tokenService.VerifyGoogleToken(googleAuth);
            if (payload == null)
                return new BadRequestObjectResult("Invalid External Authentication.");

            var info = new UserLoginInfo(googleAuth.Provider, payload.Subject, googleAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new AppUser { Email = payload.Email, UserName = payload.Email };
                    var result = await _userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        return new BadRequestObjectResult("User creation failed.");
                    }

                    // Prepare and send an email for email confirmation

                    await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            // Update payload details
            payload.JwtId = Guid.NewGuid().ToString();
            payload.Email = user.Email;
            payload.Subject = user.Id;
            payload.ExpirationTimeSeconds = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
            payload.IssuedAtTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            payload.Audience = "gistblog.com";
            payload.Issuer = "gistblog.com";

            return new OkObjectResult(payload);
        }
        catch (Exception ex)
        {
            // Log the exception details for troubleshooting
            // Return an appropriate error response
            return new StatusCodeResult(500);
        }
    }
}
