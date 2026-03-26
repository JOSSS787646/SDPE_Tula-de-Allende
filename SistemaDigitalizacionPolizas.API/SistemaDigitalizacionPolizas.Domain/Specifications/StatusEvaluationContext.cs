using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public record StatusEvaluationContext(
     bool HasObservado,
     bool AllApproved,
     bool AllLoaded,
     bool HasLoaded,
     bool HasMissing,
     bool IsExpired

 );
}
