using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Responses;

namespace GistBlog.BLL.Services.Contracts;

public interface IAuthenticationService
{
    Task<Status> Register(RegistrationDto model);
    Task<LoginResponse> Login(LoginDto model);
    Task<Status> ChangePassword(ChangePasswordDto model);
    Task<Status> AdminRegistration(RegistrationDto model);
    Task<bool> SendVerificationEmail(string email, string verificationToken);
    Task<Status> VerifyUser(string email, string verificationToken);
}
