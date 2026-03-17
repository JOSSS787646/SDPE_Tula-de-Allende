using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateCFDI
{
    public record UpdateCFDICommand
    (
        int IdRequest,
        string CFDI
    ) : IRequest<bool>;
}
