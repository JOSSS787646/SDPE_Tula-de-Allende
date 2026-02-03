
namespace SistemaDigitalizacionPolizas.Domain.Dtos.Permission
{
    public class PermissionGroupDto
    {

        //Pertenece a la Entidad Permission
        public string Module { get; set; }
        public List<string> Action { get; set; }
    }
}
