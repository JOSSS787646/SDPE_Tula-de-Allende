using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        // 🔴 ESTO ES CLAVE
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var smtp = new SmtpClient
        {
            Host = _configuration["Smtp:Host"],
            Port = int.Parse(_configuration["Smtp:Port"]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _configuration["Smtp:User"],
                _configuration["Smtp:Password"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        mail.To.Add(to);

        await smtp.SendMailAsync(mail);
    }
}
