using GistBlog.DAL.EmailServiceConfigurations.messages;
using GistBlog.DAL.EmailServiceConfigurations.services;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class EmailSenderController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public EmailSenderController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail()
    {
        var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
        var rMessage = new Message(new[] { "chukwuchidieberejohn@gmail.com" }, "Test email from John",
            "This is the content from my email.", files);
        await _emailSender.SendEmailAsync(rMessage);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Email()
    {
        var message = new Message(new[] { "chukwuchidieberejohn@gmail.com" }, "Test email from John",
            "This is the content from my email.", null);
        await _emailSender.SendEmailAsync(message);
        return Ok();
    }
}
