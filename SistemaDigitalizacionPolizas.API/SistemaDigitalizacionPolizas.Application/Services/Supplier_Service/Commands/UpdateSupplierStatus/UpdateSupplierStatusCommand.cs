using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplierStatus
{
    public record UpdateSupplierStatusCommand(string RfcSupplier, bool Active)
       : IRequest<bool>;
}
