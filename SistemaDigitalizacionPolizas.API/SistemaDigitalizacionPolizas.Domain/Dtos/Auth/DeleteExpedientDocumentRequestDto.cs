using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Auth
{
    public class DeleteExpedientDocumentRequestDto
    {
        public string Password { get; set; } = null!;
    }
}
