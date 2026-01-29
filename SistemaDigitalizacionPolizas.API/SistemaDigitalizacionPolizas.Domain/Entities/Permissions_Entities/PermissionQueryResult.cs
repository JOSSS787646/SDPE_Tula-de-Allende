using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Permissions_Entities
{
    public class PermissionQueryResult
    {
        public int IdPermiso { get; set; }
        public string Modulo { get; set; } = null!;
        public string Accion { get; set; } = null!;
        public bool Asignado { get; set; }
    }
}
