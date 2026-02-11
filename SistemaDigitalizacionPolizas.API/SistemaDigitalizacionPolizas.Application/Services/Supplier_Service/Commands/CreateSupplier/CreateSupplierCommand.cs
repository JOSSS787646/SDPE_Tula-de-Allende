

using SistemaDigitalizacionPolizas.Domain.Dtos.Supplier;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.CreateSupplier
{
    public record CreateSupplierCommand(
       SupplierDto Supplier
   ) : IRequest<int>;
}
