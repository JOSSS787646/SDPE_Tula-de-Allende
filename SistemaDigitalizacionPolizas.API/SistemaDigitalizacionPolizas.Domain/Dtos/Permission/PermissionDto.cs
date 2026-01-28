using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Permission
{
    public class PermissionDto
    {
        public int IdPermiso { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public bool Asignado { get; set; }

    }
}
