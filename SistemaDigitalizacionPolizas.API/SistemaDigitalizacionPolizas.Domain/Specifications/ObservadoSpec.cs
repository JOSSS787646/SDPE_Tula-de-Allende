using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public sealed class ObservadoSpec : IStatusSpecification
    {
        public RequestStatusEnum TargetStatus => RequestStatusEnum.Observado;

        public bool IsSatisfiedBy(StatusEvaluationContext ctx)
            => ctx.HasObservado;
    }
}
