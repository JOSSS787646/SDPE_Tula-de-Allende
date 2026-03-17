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


        Task SendDocumentReviewedAsync(
         string toEmail,
         string reviewerName,
         string requestNumber,
         string documentName,
         string result,
         string? observations
        );


        Task SendDocumentReviewNotificationAsync(
         string toEmail,
         string reviewerName,
         string requestNumber,
         string documentName,
         string result,
         string? observations
        );
    }



}
