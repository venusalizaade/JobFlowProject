using System.Net.Mail;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Services.MailKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace JobFlowProject.Business.Services.EmailSender;

public class EmailService : IEmailService
{
private readonly EmailSetting _settings;

public EmailService(IOptions<EmailSetting> options)
{
    _settings = options.Value;
}

public async Task SendAsync(string to, string subject, string body)
{
    var email = new MimeMessage();

    email.From.Add(new MailboxAddress(
        _settings.SenderName,
        _settings.SenderEmail));

    email.To.Add(MailboxAddress.Parse(to));

    email.Subject = subject;

    email.Body = new BodyBuilder
    {
        HtmlBody = body
    }.ToMessageBody();

    using var smtp = new SmtpClient();

    await smtp.ConnectAsync(
        _settings.Host,
        _settings.Port,
        _settings.EnableSsl
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.None);

    await smtp.AuthenticateAsync(
        _settings.Username,
        _settings.Password);

    await smtp.SendAsync(email);

    await smtp.DisconnectAsync(true);
}

}