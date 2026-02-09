using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetByCodeFundingSource
{
    public record GetByCodeCommunityQuery(int code)
     : IRequest<FundingSourceDto?>;
}
