using System.Security.Claims;
using GistBlog.DAL.Entities.Responses;

namespace GistBlog.BLL.Services.Contracts;

public interface ITokenService
{
    TokenResponse GetToken(IEnumerable<Claim> claims);
    string GetRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
