using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Especificación que valida si una solicitud debe marcarse como "En revisión".
/// 
/// Se cumple cuando todos los documentos han sido cargados (AllLoaded),
/// no existen observaciones, la solicitud no está vencida y aún no todos
/// los elementos han sido aprobados (AllApproved == false).
/// 
/// Define el estado objetivo "EnRevision" dentro del flujo de evaluación
/// de estados usando el patrón Specification.
/// </summary>
/// 


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
