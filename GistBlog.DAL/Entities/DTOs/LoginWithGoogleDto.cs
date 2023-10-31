using Microsoft.AspNetCore.Authentication;

namespace GistBlog.DAL.Entities.DTOs;

public class LoginWithGoogleDto
{
    // public string GoogleId { get; set; }
    // public string Email { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
    public IList<AuthenticationScheme> ExternalLogins { get; set; }
}
