using GistBlog.DAL.Configurations.EmailConfig.messages;

namespace GistBlog.DAL.Configurations.EmailConfig.services;

public interface IEmailSender
{
    void SendEmail(Message message);
    Task SendEmailAsync(Message message);
}