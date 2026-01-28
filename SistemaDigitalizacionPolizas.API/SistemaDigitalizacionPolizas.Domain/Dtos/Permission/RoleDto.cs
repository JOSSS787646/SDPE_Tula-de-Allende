using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Permission
{
    public class RoleDto
    {

        public int IdRol { get; set; }
        public string RolName { get; set; }
        public string Description { get; set; }
        public bool active { get; set; }

    }
}
