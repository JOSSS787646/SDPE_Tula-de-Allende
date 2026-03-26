using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public interface IStatusSpecification
    {
        RequestStatusEnum TargetStatus { get; }
        bool IsSatisfiedBy(StatusEvaluationContext ctx);
    }
}
