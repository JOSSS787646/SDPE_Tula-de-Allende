using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Interfaz que define el contrato para evaluar reglas de cambio de estado
/// de una solicitud.
///
/// Obliga a implementar:
/// - El estado objetivo (TargetStatus).
/// - La lógica de validación (IsSatisfiedBy) basada en un contexto.
///
/// Forma parte del patrón Specification, permitiendo encapsular y
/// desacoplar las reglas de negocio para la transición de estados.
/// </summary>
/// 


namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public interface IStatusSpecification
    {
        RequestStatusEnum TargetStatus { get; }
        bool IsSatisfiedBy(StatusEvaluationContext ctx);
    }
}
