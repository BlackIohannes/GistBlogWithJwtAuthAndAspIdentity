using GistBlog.DAL.Configurations.EmailConfig.messages;
using GistBlog.DAL.Configurations.EmailConfig.services;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class ForgotPasswordController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;

    public ForgotPasswordController(IEmailSender emailSender, UserManager<AppUser> userManager)
    {
        _emailSender = emailSender;
        _userManager = userManager;
    }

    [HttpPost("forgot-password-async")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null)
            return BadRequest("Invalid Request");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", forgotPasswordDto.Email }
        };

        // var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
        var callback = Url.Action(nameof(ForgotPassword), "ForgotPassword", new { token, email = user.Email });
        var message = new Message(new[] { user.Email }, "Reset password token", callback, null);

        await _emailSender.SendEmailAsync(message);

        return Ok($"Use The Generated Token Below To Reset Password:\n\n {token}");
    }
}