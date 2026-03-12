using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string code)
    {
        // 🔐 Seguridad TLS
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        // 📦 Obtener assembly actual (Infrastructure)
        var assembly = Assembly.GetExecutingAssembly();


 

        var resources = assembly.GetManifestResourceNames();
        foreach (var r in resources)
        {
            Console.WriteLine("RESOURCE => " + r);
        }

        // 📄 Nombre COMPLETO del recurso embebido
        var resourceName =
   "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.PasswordRecovery.html";







        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
            throw new FileNotFoundException(
                $"No se encontró el template embebido: {resourceName}"
            );

        using var reader = new StreamReader(stream);
        var htmlBody = await reader.ReadToEndAsync();

        // 🔁 Reemplazar código
        htmlBody = htmlBody.Replace("{{CODE}}", code);

        // 📧 Configuración SMTP
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
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtp.SendMailAsync(mail);
    }


    public async Task SendDocumentsUploadedAsync(
    string to,
    string userName,
    int requestId,
    DateTime date
)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var assembly = Assembly.GetExecutingAssembly();

        var resourceName =
        "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.DocumentsUploaded.html";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
            throw new Exception("No se encontró el template.");

        using var reader = new StreamReader(stream);

        var htmlBody = await reader.ReadToEndAsync();

        htmlBody = htmlBody
            .Replace("{{USER}}", userName)
            .Replace("{{REQUEST}}", requestId.ToString())
            .Replace("{{DATE}}", date.ToString("dd/MM/yyyy"))
            .Replace("{{TIME}}", date.ToString("HH:mm"));

        var smtp = new SmtpClient
        {
            Host = _configuration["Smtp:Host"],
            Port = int.Parse(_configuration["Smtp:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _configuration["Smtp:User"],
                _configuration["Smtp:Password"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]),
            Subject = "Documentos cargados para revisión",
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtp.SendMailAsync(mail);
    }
}
