using GistBlog.DAL.Configurations.EmailConfig.messages;
using GistBlog.DAL.Configurations.EmailConfig.services;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1")]
public class EmailSenderController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public EmailSenderController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var message = new Message(new[] { "chukwuchidieberejohn@gmail.com" }, "Test email from John",
            "This is the content from my email.", null);
        await _emailSender.SendEmailAsync(message);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
        var message = new Message(new[] { "chukwuchidieberejohn@gmail.com" }, "Test email from John",
            "This is the content from my email.", files);
        await _emailSender.SendEmailAsync(message);
        return Ok();
    }
}