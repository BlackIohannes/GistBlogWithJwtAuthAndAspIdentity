namespace GistBlog.BLL.Services.Contracts;

public interface IResetPasswordService
{
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
}