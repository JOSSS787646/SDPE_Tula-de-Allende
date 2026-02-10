using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateStatusCommunity
{
    public record UpdateStatusCommunityCommand
    (int Code,
        bool Active
    ) : IRequest<bool>;
}
