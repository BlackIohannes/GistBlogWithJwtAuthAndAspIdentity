namespace GistBlog.DAL.Entities.DTOs;

public class PasswordResetInfoDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}