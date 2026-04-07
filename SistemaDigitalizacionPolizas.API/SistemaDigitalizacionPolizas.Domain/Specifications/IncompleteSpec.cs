using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Especificación que valida si una solicitud debe marcarse como "Incompleto".
/// 
/// Se cumple cuando la solicitud está vencida, tiene elementos faltantes
/// o no todos los documentos han sido cargados, siempre y cuando no existan
/// observaciones (HasObservado == false).
/// 
/// Define el estado objetivo "Incompleto" dentro del flujo de evaluación
/// de estados usando el patrón Specification.
/// </summary>
/// 


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
