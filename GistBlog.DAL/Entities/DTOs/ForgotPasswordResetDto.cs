namespace GistBlog.DAL.Entities.DTOs;

public class ForgotPasswordResetDto
{
    public string Email { get; set; }
    public string CallbackUrl { get; set; }
}