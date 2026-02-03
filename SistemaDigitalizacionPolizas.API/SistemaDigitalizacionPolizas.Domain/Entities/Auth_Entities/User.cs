using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;

public class User
{
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Asset { get; set; }   // BIT → bool
    public DateTime? LastAccess { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public int IdAdministrativeUnit { get; set; }
    public int IdRole { get; set; }

    public AdministrativeUnit AdministrativeUnit { get; set; }
    public Role Role { get; set; }
}
