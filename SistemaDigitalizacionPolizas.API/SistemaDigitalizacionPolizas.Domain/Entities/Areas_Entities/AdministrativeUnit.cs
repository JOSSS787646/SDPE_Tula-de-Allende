

namespace SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities
{
    public class AdministrativeUnit
    {
        public int IdAdministrativeUnit { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }

        // 1 unidad → muchos usuarios
        public ICollection<User> Users { get; set; }

    }
}
