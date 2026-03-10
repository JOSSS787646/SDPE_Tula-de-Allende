using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy
{
    public record DownloadPaymentPolicyDto(
       string FileName,
       string ContentType,
       Stream FileStream
   );
}

