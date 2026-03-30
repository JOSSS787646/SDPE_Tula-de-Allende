using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Services
{
    public class RequestStatusEvaluator
    {
        // Orden = prioridad de negocio
        private static readonly IReadOnlyList<IStatusSpecification> _specs =
            new List<IStatusSpecification>
            {
                new ObservationSpec(),
                new CompleteSpec(),
                new UnderReviewSpec(),
                new IncompleteSpec()
            };

        public RequestStatusEnum Evaluate(StatusEvaluationContext ctx)
        {
            foreach (var spec in _specs)
            {
                if (spec.IsSatisfiedBy(ctx))
                    return spec.TargetStatus;
            }

            return RequestStatusEnum.Incompleto; 
        }
    }
}
