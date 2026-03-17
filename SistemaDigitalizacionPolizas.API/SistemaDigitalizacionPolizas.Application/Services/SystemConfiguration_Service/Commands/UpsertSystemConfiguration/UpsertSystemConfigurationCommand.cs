using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Commands.UpsertSystemConfiguration
{
    public record UpsertSystemConfigurationCommand(
       bool EmailsEnabled,
       DateTime? NotificationStartDate
   ) : IRequest<int>;
}
