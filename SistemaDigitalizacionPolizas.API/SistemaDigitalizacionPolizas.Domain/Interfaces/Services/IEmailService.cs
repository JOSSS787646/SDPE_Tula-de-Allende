namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);

       Task SendDocumentsUploadedAsync(
        string to,
        string userName,
        int requestId,
        DateTime date);
    }
}
