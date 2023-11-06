using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.Models.UserEntities;
using Microsoft.AspNetCore.Identity;

namespace GistBlog.BLL.Services.Implementation;

public class ResetPasswordService : IResetPasswordService
{
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var resetPassResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!resetPassResult.Succeeded)
            // Log or handle errors if needed
            return false;

        await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        return true;
    }
}