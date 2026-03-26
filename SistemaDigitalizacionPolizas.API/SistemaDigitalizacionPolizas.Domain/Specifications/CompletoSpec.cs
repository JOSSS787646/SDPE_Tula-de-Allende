using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public sealed class CompletoSpec : IStatusSpecification
    {
        public RequestStatusEnum TargetStatus => RequestStatusEnum.Completo;

        public bool IsSatisfiedBy(StatusEvaluationContext ctx)
            => ctx.AllApproved;
    }
}
