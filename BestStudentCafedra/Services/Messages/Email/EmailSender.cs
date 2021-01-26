using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Services.Messages.Email
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string Content { get; set; }
    }
    public abstract class EmailSender : IMessageSender<List<string>, EmailMessage>
    {
        public async Task SendAsync(List<string> to, EmailMessage message)
        {
            await SendEmailAsync(to, message);
        }

        abstract public Task SendEmailAsync(List<string> adresses, EmailMessage message);
    }
}
