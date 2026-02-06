using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UpdateDataUser
    {
        public int IdUser { get; set; }

        public string Email { get; set; } = null!;

        public int IdRole { get; set; }

        public int IdAdministrativeUnit { get; set; }
    }
}
