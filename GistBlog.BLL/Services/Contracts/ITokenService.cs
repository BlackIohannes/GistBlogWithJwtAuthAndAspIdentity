using System.Security.Claims;
using GistBlog.DAL.Entities.DTOs.socials;
using GistBlog.DAL.Entities.Responses;
using Google.Apis.Auth;

namespace GistBlog.BLL.Services.Contracts;

public interface ITokenService
{
    TokenResponse GetToken(IEnumerable<Claim> claims);
    string GetRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(GoogleAuthDto googleAuth);
}
