using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using MailKit.Net.Smtp;
using MimeKit;

namespace API.Interfaces
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration=emailConfiguration;
        }
        public bool SendEmail(EMailMessage message)
        {
            bool success=false;
         var emailMessage = CreateEmailMessage(message);
          success= Send(emailMessage);
          return success;
        }

        private MimeMessage CreateEmailMessage(EMailMessage message)
{
    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("PharmaSys",_emailConfiguration.From));
    emailMessage.To.AddRange(message.To);
    emailMessage.Subject = message.Subject;
    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
    return emailMessage;
}
private bool Send(MimeMessage mailMessage)
{
    bool success=false;
    using (var client = new SmtpClient())
    {
        try
        {
            client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
            client.Send(mailMessage);
        }
        catch
        {
            //log an error message or throw an exception or both.
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
            success=true;
        }
        return success;
    }
}

     
    }
}