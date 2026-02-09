using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.CreateCommunity
{
    public record CreateCommunityCommand
     (
      int Code,
      string Description,
      bool Active
    ) : IRequest<int>;
}
