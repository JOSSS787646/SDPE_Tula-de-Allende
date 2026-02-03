namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UserSessionDto
    {
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AdministrativeUnit { get; set; }
    }
}
