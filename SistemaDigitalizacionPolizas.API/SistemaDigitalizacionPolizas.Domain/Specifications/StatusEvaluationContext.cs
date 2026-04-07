using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Contexto de evaluación que agrupa las condiciones necesarias para
/// determinar el estado de una solicitud.
///
/// Contiene indicadores como observaciones, aprobaciones, carga de documentos,
/// elementos faltantes y expiración, los cuales son utilizados por las
/// especificaciones (Specification) para validar transiciones de estado.
/// </summary>
/// 


namespace SistemaDigitalizacionPolizas.Domain.Specifications
{
    public record StatusEvaluationContext(
     bool HasObservado,
     bool AllApproved,
     bool AllLoaded,
     bool HasLoaded,
     bool HasMissing,
     bool IsExpired

 );
}
