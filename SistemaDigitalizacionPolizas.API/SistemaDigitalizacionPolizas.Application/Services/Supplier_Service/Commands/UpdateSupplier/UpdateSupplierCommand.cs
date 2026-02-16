using SistemaDigitalizacionPolizas.Domain.Dtos.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplier
{
    public record UpdateSupplierCommand(SupplierDto Supplier)
      : IRequest<bool>;
}
