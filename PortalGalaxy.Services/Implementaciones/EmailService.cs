using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Shared.Configuracion;

namespace PortalGalaxy.Services.Implementaciones;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpConfiguration _smtpConfiguration;
    
    public EmailService(ILogger<EmailService> logger, IOptions<AppSettings> options)
    {
        _logger = logger;
        _smtpConfiguration = options.Value.SmtpConfiguration;
    }
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var mailMessage =
                new MailMessage(new MailAddress(_smtpConfiguration.Usuario, _smtpConfiguration.Remitente)
                , new MailAddress(email));
            
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            using var smtpClient = new SmtpClient(_smtpConfiguration.Servidor, _smtpConfiguration.Puerto);
            
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_smtpConfiguration.Usuario, _smtpConfiguration.Password);
            smtpClient.EnableSsl = _smtpConfiguration.UsarSsl;

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando email a {email} {message}", email, ex.Message);
        }
    }
}