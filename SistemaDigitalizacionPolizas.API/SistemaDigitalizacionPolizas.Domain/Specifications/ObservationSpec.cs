using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Especificación que valida si una solicitud debe marcarse como "Observado".
/// 
/// Se cumple cuando existe al menos una observación registrada en la solicitud
/// (ctx.HasObservado == true).
/// 
/// Define el estado objetivo "Observado" dentro del flujo de evaluación
/// de estados utilizando el patrón Specification.
/// </summary>
/// 


namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public sealed class ObservationSpec : IStatusSpecification
    {
        public RequestStatusEnum TargetStatus => RequestStatusEnum.Observado;

        public bool IsSatisfiedBy(StatusEvaluationContext ctx)
            => ctx.HasObservado;
    }
}
