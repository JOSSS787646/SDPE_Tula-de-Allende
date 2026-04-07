using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // ─────────────────────────────────────────────────────────────
    // HELPERS PRIVADOS
    // ─────────────────────────────────────────────────────────────

    private SmtpClient BuildSmtpClient() =>
        new SmtpClient
        {
            Host = _configuration["Smtp:Host"]!,
            Port = int.Parse(_configuration["Smtp:Port"]!),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _configuration["Smtp:User"],
                _configuration["Smtp:Password"]
            )
        };

    private async Task<string> LoadTemplateAsync(string resourceName)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException(
                $"No se encontró el template embebido: {resourceName}");

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Badge superior de la tarjeta:
    /// Aprobado → fondo verde claro  |  Observado → fondo rojo claro
    /// </summary>
    private static string BuildBadge(string result)
    {
        bool aprobado = result.Equals("Aprobado", StringComparison.OrdinalIgnoreCase);

        var bg = aprobado ? "#e6f4ea" : "#fdecea";
        var border = aprobado ? "#b7dfbe" : "#f5c6c2";
        var badgeBg = aprobado ? "#2e7d32" : "#c62828";
        var label = aprobado ? "✔ DOCUMENTO APROBADO" : "⚠ DOCUMENTO OBSERVADO";

        return $@"
<tr>
  <td style=""background:{bg};border-bottom:1px solid {border};padding:12px 30px;text-align:center;"">
    <span style=""display:inline-block;background:{badgeBg};color:#fff;font-size:12px;font-weight:600;padding:4px 14px;border-radius:20px;letter-spacing:.4px;"">
      {label}
    </span>
  </td>
</tr>";
    }

    /// <summary>
    /// Span coloreado para la celda "Resultado":
    /// Aprobado → verde  |  Observado → rojo
    /// </summary>
    private static string BuildResultSpan(string result)
    {
        bool aprobado = result.Equals("Aprobado", StringComparison.OrdinalIgnoreCase);

        var color = aprobado ? "#1b5e20" : "#b71c1c";
        var bgSpan = aprobado ? "#e8f5e9" : "#ffebee";
        var icon = aprobado ? "✔" : "⚠";

        return $@"<span style=""background:{bgSpan};color:{color};font-weight:700;padding:2px 10px;border-radius:12px;font-size:13px;"">{icon} {result}</span>";
    }

    /// <summary>
    /// Bloque de observaciones:
    /// - Si fue Aprobado → no se renderiza (string vacío)
    /// - Si fue Observado → bloque rojo con el texto
    /// </summary>
    private static string BuildObservationsBlock(string result, string? observations)
    {
        bool aprobado = result.Equals("Aprobado", StringComparison.OrdinalIgnoreCase);

        if (aprobado)
            return string.Empty;

        var text = string.IsNullOrWhiteSpace(observations)
            ? "Sin observaciones registradas."
            : observations;

        return $@"
<div style=""background:#fff5f5;border:1px solid #f5c6c2;border-left:4px solid #c62828;border-radius:10px;padding:20px;margin-bottom:20px;"">
  <p style=""margin:0 0 12px;font-size:13px;font-weight:700;color:#c62828;text-transform:uppercase;letter-spacing:.5px;"">
    💬 Observaciones del revisor
  </p>
  <div style=""font-size:13px;line-height:1.9;color:#444;"">
    {text}
  </div>
</div>";
    }

    // ─────────────────────────────────────────────────────────────
    // 1. Recuperación de contraseña
    // ─────────────────────────────────────────────────────────────

    public async Task SendAsync(string to, string subject, string code)
    {
        var resourceName =
            "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.PasswordRecovery.html";

        var htmlBody = await LoadTemplateAsync(resourceName);
        htmlBody = htmlBody.Replace("{{CODE}}", code);

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]!),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        mail.To.Add(to);

        using var smtp = BuildSmtpClient();
        await smtp.SendMailAsync(mail);
    }

    // ─────────────────────────────────────────────────────────────
    // 2. Documentos cargados
    // ─────────────────────────────────────────────────────────────

    public async Task SendDocumentsUploadedAsync(
        string to,
        string userName,
        string requestId,
        string administrativeUnit,
        string requestDescription,
        DateTime date,
        string documentsList
    )
    {
        var resourceName =
            "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.DocumentsUploaded.html";

        var htmlBody = await LoadTemplateAsync(resourceName);

        var displayName = string.IsNullOrWhiteSpace(userName)
            ? "Usuario Adquisiciones (sin nombre)"
            : userName;

        htmlBody = htmlBody
            .Replace("{{USER}}", displayName)
            .Replace("{{REQUEST}}", requestId)
            .Replace("{{UNIT}}", administrativeUnit)
            .Replace("{{DESCRIPTION}}", requestDescription)
            .Replace("{{DATE}}", date.ToString("dd/MM/yyyy"))
            .Replace("{{TIME}}", date.ToString("HH:mm"))
            .Replace("{{DOCUMENTS}}", documentsList);

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]!),
            Subject = $"Documentos cargados para revisión - Solicitud #{requestId}",
            Body = htmlBody,
            IsBodyHtml = true
        };
        mail.To.Add(to);

        using var smtp = BuildSmtpClient();
        await smtp.SendMailAsync(mail);
    }

    // ─────────────────────────────────────────────────────────────
    // 3. Notificación al revisor (Tesorería / ReadView)
    // ─────────────────────────────────────────────────────────────

    public async Task SendDocumentReviewNotificationAsync(
        string toEmail,
        string reviewerName,
        string requestNumber,
        string result,
        string? observations,
        string documentName
    )
    {
        var resourceName =
            "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.DocumentReviewNotification.html";

        var htmlBody = await LoadTemplateAsync(resourceName);
        var now = DateTime.Now;

        htmlBody = htmlBody
            .Replace("{{BADGE}}", BuildBadge(result))
            .Replace("{{REVIEWER}}", reviewerName)
            .Replace("{{REQUEST}}", requestNumber)
            .Replace("{{DOCUMENT}}", documentName)
            .Replace("{{RESULT}}", BuildResultSpan(result))
            .Replace("{{OBSERVATIONS_BLOCK}}", BuildObservationsBlock(result, observations))
            .Replace("{{DATE}}", now.ToString("dd/MM/yyyy"))
            .Replace("{{TIME}}", now.ToString("HH:mm"));

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]!),
            Subject = $"Documentos revisados - Solicitud #{requestNumber}",
            Body = htmlBody,
            IsBodyHtml = true
        };
        mail.To.Add(toEmail);

        using var smtp = BuildSmtpClient();
        await smtp.SendMailAsync(mail);
    }

    // ─────────────────────────────────────────────────────────────
    // 4. Resultado al cargador (Adquisiciones)
    // ─────────────────────────────────────────────────────────────

    public async Task SendDocumentReviewedAsync(
        string toEmail,
        string reviewerName,
        string requestNumber,
        string documentName,
        string result,
        string? observations
    )
    {
        var resourceName =
            "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.DocumentReviewed.html";

        var htmlBody = await LoadTemplateAsync(resourceName);
        var now = DateTime.Now;

        htmlBody = htmlBody
            .Replace("{{BADGE}}", BuildBadge(result))
            .Replace("{{REVIEWER}}", reviewerName)
            .Replace("{{REQUEST}}", requestNumber)
            .Replace("{{DOCUMENT}}", documentName)
            .Replace("{{RESULT}}", BuildResultSpan(result))
            .Replace("{{OBSERVATIONS_BLOCK}}", BuildObservationsBlock(result, observations))
            .Replace("{{DATE}}", now.ToString("dd/MM/yyyy"))
            .Replace("{{TIME}}", now.ToString("HH:mm"));

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]!),
            Subject = $"Resultado de revisión de documento - Solicitud #{requestNumber}",
            Body = htmlBody,
            IsBodyHtml = true
        };
        mail.To.Add(toEmail);

        using var smtp = BuildSmtpClient();
        await smtp.SendMailAsync(mail);
    }



    public async Task SendGroupedExpiredNotificationAsync(
       string to,
       string userName,
       List<ExpiredRequestDto> requests)
    {
        var resourceName =
            "SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Email.Templates.ExpiredRequestsGrouped.html";

        var htmlBody = await LoadTemplateAsync(resourceName);

        var requestsBlock = BuildRequestsBlock(requests);

        htmlBody = htmlBody
            .Replace("{{USER}}", userName)
            .Replace("{{REQUESTS_BLOCK}}", requestsBlock);

        var mail = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:User"]!),
            Subject = $"⚠ Solicitudes vencidas pendientes ({requests.Count})",
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        using var smtp = BuildSmtpClient();
        await smtp.SendMailAsync(mail);
    }

    private static string BuildRequestsBlock(List<ExpiredRequestDto> requests)
    {
        return string.Join("", requests.Select(r =>
        {
          

            return $@"
        <div style='
        background:#fafafa;
        border:1px solid #e8e8e8;
        border-left:4px solid #5b0b24;
        border-radius:10px;
        padding:20px;
        margin-bottom:18px;
        '>

            <p style='margin:0 0 10px;font-size:13px;font-weight:700;color:#5b0b24;'>
                Solicitud #{r.RequestNumber}
            </p>

            <ul style='padding-left:18px;font-size:13px;color:#444;'>
            
            </ul>

        </div>";
        }));
    }
}