using GistBlog.DAL.EmailServiceConfigurations.messages;

namespace GistBlog.DAL.EmailServiceConfigurations.services;

public interface IEmailSender
{
    void SendEmail(Message message);
    Task SendEmailAsync(Message message);
}