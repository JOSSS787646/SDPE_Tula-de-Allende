namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
