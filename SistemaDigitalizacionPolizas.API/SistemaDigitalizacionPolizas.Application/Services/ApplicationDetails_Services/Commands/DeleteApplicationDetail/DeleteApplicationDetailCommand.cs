using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.DeleteApplicationDetail
{
    public record DeleteApplicationDetailCommand(int IdDetail) : IRequest;
}
