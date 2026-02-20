using SistemaDigitalizacionPolizas.Domain.Dtos.Beneficiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateBeneficiary
{
    public record UpdateBeneficiaryCommand(
     int IdBeneficiary,
     BeneficiaryDto Beneficiary
 ) : IRequest<bool>;
}
