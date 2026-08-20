using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Services.MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace JobFlowProject.Business.Services.EmailSender;

public class EmailService : IEmailService
{
    private readonly EmailSetting _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSetting> options, ILogger<EmailService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        ValidateSettings();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

        var smtp = new SmtpClient
        {
            ServerCertificateValidationCallback = (s, c, h, e) => true
        };

        try
        {
            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                _settings.EnableSsl ? SecureSocketOptions.StartTlsWhenAvailable : SecureSocketOptions.None);

            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);

            await smtp.SendAsync(email);

            _logger.LogInformation("Email sent to {To} with subject '{Subject}'", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To} with subject '{Subject}'", to, subject);
            throw;
        }
        finally
        {
            if (smtp.IsConnected)
            {
                await smtp.DisconnectAsync(true);
            }
            smtp.Dispose();
        }
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_settings.Host))
            throw new InvalidOperationException("EmailSettings:Host is not configured. Check appsettings.json or environment variables (EmailSettings__Host).");

        if (string.IsNullOrWhiteSpace(_settings.SenderEmail))
            throw new InvalidOperationException("EmailSettings:SenderEmail is not configured.");

        if (string.IsNullOrWhiteSpace(_settings.Password))
            throw new InvalidOperationException("EmailSettings:Password is not configured. Set the Gmail app password via environment variable EmailSettings__Password.");

        if (_settings.Port == 0)
            _settings.Port = 587;
    }
}
