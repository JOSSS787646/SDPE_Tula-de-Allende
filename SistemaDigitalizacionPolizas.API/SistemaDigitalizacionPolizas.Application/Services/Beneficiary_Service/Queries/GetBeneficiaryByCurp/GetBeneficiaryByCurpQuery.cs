using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetBeneficiaryByCurp
{
    public record GetBeneficiaryByCurpQuery(string Curp)
    : IRequest<BeneficiaryDto?>;
}
