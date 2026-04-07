using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Especificación que valida si una solicitud puede cambiar al estado "Completo".
/// 
/// La condición se cumple únicamente cuando todas las validaciones o documentos
/// asociados han sido aprobados (ctx.AllApproved == true).
/// 
/// Define explícitamente que el estado objetivo es "Completo" y forma parte
/// del flujo de evaluación de estados usando el patrón Specification.
/// </summary>
/// 

namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public sealed class CompleteSpec : IStatusSpecification
    {
        public RequestStatusEnum TargetStatus => RequestStatusEnum.Completo;

        public bool IsSatisfiedBy(StatusEvaluationContext ctx)
            => ctx.AllApproved;
    }
}
