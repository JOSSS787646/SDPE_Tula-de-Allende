using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateStatusBeneficiary
{
    public record UpdateBeneficiaryStatusCommand(string Curp, bool Active) : IRequest<bool>;
}
