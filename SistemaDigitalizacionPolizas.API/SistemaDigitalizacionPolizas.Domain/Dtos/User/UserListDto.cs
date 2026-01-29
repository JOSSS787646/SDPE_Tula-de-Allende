using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UserListDto
    {
        public int IdUser { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string AdministrativeUnit { get; set; } = null!;
        public bool Asset { get; set; }
    }
}
