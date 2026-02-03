namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UpdateUserDto
    {
        public int IdUser { get; set; }
        public string NewEmail { get; set; } = null!;
    }
}
