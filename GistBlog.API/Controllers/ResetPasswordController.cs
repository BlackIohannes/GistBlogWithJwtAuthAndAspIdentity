using GistBlog.DAL.Configurations.EmailConfig.services;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/reset-password")]
public class ResetPasswordController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordController(IEmailSender emailSender, UserManager<AppUser> userManager)
    {
        _emailSender = emailSender;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModelDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            return BadRequest("Invalid Request");

        var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
        if (!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(e => e.Description);

            return BadRequest(new { Errors = errors });
        }

        await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        return Ok("Password Reset Was Successful!");
    }
}