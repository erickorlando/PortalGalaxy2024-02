using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Shared.Configuracion;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            var client = new SendGridClient(_smtpConfiguration.Password);
            var from = new EmailAddress(_smtpConfiguration.Usuario, _smtpConfiguration.Remitente);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.DeserializeResponseBodyAsync(response.Body);
                
                _logger.LogError(result.ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando email a {email} {message}", email, ex.Message);
        }
    }
}