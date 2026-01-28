using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    public class UpdatePasswordDto
    {
        public int IdUser { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}
