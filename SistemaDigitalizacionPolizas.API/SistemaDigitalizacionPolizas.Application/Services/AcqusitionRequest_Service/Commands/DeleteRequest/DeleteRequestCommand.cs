using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.DeleteRequest
{
    public record DeleteRequestCommand(
        int RequestId,
            string Password
        ) : IRequest<bool>;
}
