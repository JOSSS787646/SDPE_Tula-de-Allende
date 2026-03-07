using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager
{
    public class RequestManagerPreviewDto
    {
        public int IdRequestManager { get; set; }

        public string FullName { get; set; } = null!;

        public string? RequestNumber { get; set; }

        public string? AdministrativeUnit { get; set; }
    }
}
