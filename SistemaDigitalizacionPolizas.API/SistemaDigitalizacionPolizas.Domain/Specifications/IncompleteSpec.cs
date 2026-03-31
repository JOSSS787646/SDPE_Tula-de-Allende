using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public sealed class IncompleteSpec : IStatusSpecification
    {
        public RequestStatusEnum TargetStatus => RequestStatusEnum.Incompleto;

        public bool IsSatisfiedBy(StatusEvaluationContext ctx)
            => (ctx.IsExpired || ctx.HasMissing || !ctx.AllLoaded) 
               && !ctx.HasObservado;
    }
}
