using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities
{
    public class PasswordResetToken
    {
        public int IdToken { get; set; }
        public int IdUser { get; set; }
        public string Code { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
        public bool Used { get; set; }
        public DateTime DateCreate { get; set; }

        // Navegación
        public User User { get; set; } = null!;
    }
}
