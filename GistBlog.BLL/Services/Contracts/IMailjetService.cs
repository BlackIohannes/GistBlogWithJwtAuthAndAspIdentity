namespace GistBlog.BLL.Services.Contracts;

public interface IMailjetService
{
    Task SendEmailAsync(string toEmail, string toName, string subject, string content);
}
