
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateFundingSource
{
    public record UpdateFundingSourceCommand
    (
     int Code,
     string Description,
     bool Active
     ): IRequest<bool>;
}
