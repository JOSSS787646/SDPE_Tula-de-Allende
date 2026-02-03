using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateStateFunding
{
    public record UpdateStateFundingCommand(int Code) : IRequest<bool>;
    
}
