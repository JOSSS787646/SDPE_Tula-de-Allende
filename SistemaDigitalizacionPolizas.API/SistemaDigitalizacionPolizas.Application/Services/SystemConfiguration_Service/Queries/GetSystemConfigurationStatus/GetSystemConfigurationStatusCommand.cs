using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Queries.GetSystemConfigurationStatus
{

    public record GetSystemConfigurationStatusCommand()
        : IRequest<GetSystemConfigurationStatusResponse>;

    public record GetSystemConfigurationStatusResponse(
        bool Status,
        string Date
    );
}
