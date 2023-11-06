namespace GistBlog.BLL.Services.Contracts;

public interface IPasswordResetService
{
    Task<string?> GeneratePasswordResetTokenAsync(string email);
    Task<bool> SendPasswordResetEmailAsync(string email, string callbackUrl);
}