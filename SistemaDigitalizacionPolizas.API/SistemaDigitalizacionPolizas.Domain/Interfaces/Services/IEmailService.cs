namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);

        Task SendDocumentsUploadedAsync(
        string to,
        string userName,
        string requestId,
        string administrativeUnit,
        string requestDescription,
        DateTime date,
        string documentsList);
    }
}
