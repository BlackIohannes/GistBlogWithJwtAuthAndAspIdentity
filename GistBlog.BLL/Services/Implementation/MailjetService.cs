using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.Models.Domain;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace GistBlog.BLL.Services.Implementation;

public class MailjetService : IMailjetService
{
    private readonly MailjetSettings _mailjetSettings;

    public MailjetService(IOptions<MailjetSettings> mailjetSettings)
    {
        _mailjetSettings = mailjetSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string toName, string subject, string content)
    {
        var client = new MailjetClient(_mailjetSettings.ApiKey, _mailjetSettings.ApiSecret);
        var request = new MailjetRequest
        {
            Resource = SendV31.Resource
        };

        request.Property(Send.Messages, new JArray
        {
            new JObject
            {
                {
                    "From",
                    new JObject
                    {
                        { "Email", _mailjetSettings.EmailSender },
                        { "Name", "Sender" }
                    }
                },
                {
                    "To",
                    new JArray
                    {
                        new JObject
                        {
                            { "Email", toEmail },
                            { "Name", toName }
                        }
                    }
                },
                { "Subject", subject },
                { "HTMLPart", content }
            }
        });

        await client.PostAsync(request);
    }
}
