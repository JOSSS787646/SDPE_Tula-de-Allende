using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public sealed class UnderReviewSpec : IStatusSpecification
    {
        public RequestStatusEnum TargetStatus => RequestStatusEnum.EnRevision;

        public bool IsSatisfiedBy(StatusEvaluationContext ctx)
            => ctx.AllLoaded
               && !ctx.HasObservado
               && !ctx.IsExpired
               && !ctx.AllApproved;
    }
}
