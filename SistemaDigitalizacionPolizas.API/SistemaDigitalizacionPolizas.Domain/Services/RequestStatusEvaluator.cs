using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Servicio encargado de determinar el estado final de una solicitud
/// evaluando un conjunto de especificaciones con prioridad definida.
///
/// Recorre las reglas (Specification) en orden y retorna el primer estado
/// cuya condición se cumpla, asegurando coherencia en la lógica de negocio.
///
/// El orden de las especificaciones representa la prioridad de evaluación,
/// evitando conflictos entre estados.
///
/// Si ninguna regla se cumple, asigna por defecto el estado "Incompleto".
/// </summary>
/// 


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
