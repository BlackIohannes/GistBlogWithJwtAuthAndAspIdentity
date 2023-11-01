﻿using Microsoft.AspNetCore.Http;
using MimeKit;

namespace GistBlog.DAL.EmailServiceConfigurations.messages;

public class Message
{
    public Message(IEnumerable<string> to, string subject, string? content, IFormFileCollection? attachments)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress("email", x)));
        Subject = subject;
        Content = content;
        Attachments = attachments;
    }

    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string? Content { get; set; }

    public IFormFileCollection? Attachments { get; set; }
}