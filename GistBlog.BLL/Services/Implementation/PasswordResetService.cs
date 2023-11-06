using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations.EmailConfig.messages;
using GistBlog.DAL.Configurations.EmailConfig.services;
using GistBlog.DAL.Entities.Models.UserEntities;
using Microsoft.AspNetCore.Identity;

namespace GistBlog.BLL.Services.Implementation;

public class PasswordResetService : IPasswordResetService
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;

    public PasswordResetService(UserManager<AppUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null ? await _userManager.GeneratePasswordResetTokenAsync(user) : null;
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email, string callbackUrl)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var message = new Message(new[] { email }, "Reset password token", callbackUrl, null);
        await _emailSender.SendEmailAsync(message);

        return true;
    }
}