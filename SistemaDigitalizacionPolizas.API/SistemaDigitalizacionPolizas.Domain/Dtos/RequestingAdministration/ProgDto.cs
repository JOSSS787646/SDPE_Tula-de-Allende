
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration
{
    public class ProgDto
    {
        public int idProg { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

    }
}
