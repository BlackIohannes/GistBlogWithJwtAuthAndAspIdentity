using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.BLL.Services.Contracts;

public interface IAuthenticationService
{
    Task<Status> SignupAsync(RegistrationDto model);
    Task<LoginResponse> LoginAsync(LoginDto model);
    Task<Status> LogoutAsync(string username);
    Task<Status> LoginStatusAsync(string username);
    Task<Status> ChangePasswordAsync(ChangePasswordDto model);
    Task<Status> AdminRegistrationAsync(RegistrationDto model);
    Task<Status> CreateRolesAsync([FromBody] List<string> roles);
    Task<Status> AssignRolesAsync([FromBody] List<string> roles, string username);
    Task<Status> EditRoleAsync([FromBody] EditRoleDto model);
    Task<Status> DeleteRoleAsync(string roleName);
    Task<Status> EditUserRoleAsync([FromBody] EditUserRoleDto model);
    Task<Status> DeleteUserRoleAsync([FromBody] DeleteUserRoleDto model);
    Task<List<string>?> GetAllRolesAsync();
    Task<List<string>?> GetUserRolesAsync(string username);
    Task<List<string>?> GetAllUsersAndRolesAsync();
    Task<Status> DeleteUserAsync(string username);
    Task<List<string>?> GetAllUsersAsync();

    #region forget password and reset password service implementation

    Task<string?> GeneratePasswordResetTokenAsync(string email);
    Task<bool> SendPasswordResetEmailAsync(string email, string callbackUrl);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);

    #endregion
}