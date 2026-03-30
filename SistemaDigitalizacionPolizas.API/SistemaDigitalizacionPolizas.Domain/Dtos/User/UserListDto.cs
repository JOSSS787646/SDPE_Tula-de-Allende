namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UserListDto
    {
        public int IdUser { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int idRole { get; set; } 
        public string AdministrativeUnit { get; set; } = null!;
        public bool Asset { get; set; }
        public int IdAdministrativeUnit { get; set; }
    }
}
