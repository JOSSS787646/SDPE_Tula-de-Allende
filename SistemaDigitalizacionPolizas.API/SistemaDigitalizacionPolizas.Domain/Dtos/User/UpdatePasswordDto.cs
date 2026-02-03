namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UpdatePasswordDto
    {
        public int IdUser { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}
