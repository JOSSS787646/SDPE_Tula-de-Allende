using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.User
{
    //Crea aun usuario, lo que le envio a la BD
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdAdministrativeUnit { get; set; }
        public int IdRole { get; set; }
    }
}
