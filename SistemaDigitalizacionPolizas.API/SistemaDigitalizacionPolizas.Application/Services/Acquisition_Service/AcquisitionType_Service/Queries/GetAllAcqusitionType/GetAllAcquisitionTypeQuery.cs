using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;

using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAllAcqusitionType
{
    public record GetAllAcquisitionTypeQuery
      () : IRequest<List<AcquisitionTypeDto>>;
}
