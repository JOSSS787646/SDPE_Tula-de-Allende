using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateCommunity
{
    public record UpdateCommunityCommand
     (
     int idCommunity,
     int Code,
     string Description,
     bool Active
     ) : IRequest<bool>;
}
