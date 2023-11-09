using GistBlog.DAL.Configurations.EmailConfig.configurations;
using GistBlog.DAL.Configurations.EmailConfig.messages;
using MailKit.Net.Smtp;
using MimeKit;

namespace GistBlog.DAL.Configurations.EmailConfig.services;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailSender(EmailConfiguration emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    // synchronous programming
    public void SendEmail(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    // asynchronous programming
    public async Task SendEmailAsync(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            // Card Design
            HtmlBody = $"<div style='border: 1px solid #ddd; border-radius: 5px; overflow: hidden;'>" +
                       $"<div style='background-color: #f5f5f5; padding: 10px; text-align: center;'>" +
                       $"<h2 style='color: #333;'>{message.Subject}</h2></div>"
        };

        // Main Content
        bodyBuilder.HtmlBody += $"<div style='padding: 20px;'>{message.Content}</div>";

        // Token as a Link on a Separate Line
        bodyBuilder.HtmlBody += $"<div style='background-color: #4CAF50; padding: 10px; text-align: center;'>" +
                                $"<a href='{message.Content}' style='color: white; text-decoration: none; display: block;'>" +
                                $"Use The Generated Token Below To Reset Password</a></div>";

        if (message.Attachments is not null && message.Attachments.Any())
            foreach (var attachment in message.Attachments)
            {
                using var ms = new MemoryStream();
                attachment.CopyTo(ms);
                var fileBytes = ms.ToArray();
                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }

    private void Send(object emailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

            client.Send(emailMessage as MimeMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }

    private async Task SendAsync(object emailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

            await client.SendAsync(emailMessage as MimeMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}