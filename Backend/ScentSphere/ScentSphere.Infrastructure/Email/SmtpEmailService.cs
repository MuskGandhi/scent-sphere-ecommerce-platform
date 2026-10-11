using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ScentSphere.Application.Common;
using ScentSphere.Application.Interfaces;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;
using MimeKit;


namespace ScentSphere.Infrastructure.Email
{

    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public SmtpEmailService(
            IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        //public async Task SendEmailAsync(
        //    string to,
        //    string subject,
        //    string body)
        //{
        //    try
        //    {
        //        using var client = new SmtpClient(
        //            _settings.SmtpServer,
        //            _settings.Port);

        //        client.EnableSsl = true;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential(
        //            _settings.Username,
        //            _settings.Password);

        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress(
        //                _settings.SenderEmail,
        //                _settings.SenderName),
        //            Subject = subject,
        //            Body = body,
        //            IsBodyHtml = false
        //        };

        //        mailMessage.To.Add(to);

        //        await client.SendMailAsync(mailMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //}

        public async Task SendEmailAsync(
            string to,
            string subject,
            string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    _settings.SenderName,
                    _settings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _settings.SmtpServer,
                    _settings.Port,
                    SecureSocketOptions.Auto);
                await client.AuthenticateAsync(
                    _settings.Username,
                    _settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
