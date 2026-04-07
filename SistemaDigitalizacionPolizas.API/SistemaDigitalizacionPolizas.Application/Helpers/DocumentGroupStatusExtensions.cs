using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Helpers
{


    /// <summary>
    /// Clase de extensión para convertir el enum DocumentGroupStatus
    /// a su representación en texto (label).
    ///
    /// Centraliza los nombres visibles de los estados de documentos,
    /// evitando el uso de strings hardcodeados en la aplicación.
    ///
    /// Se utiliza principalmente para mostrar estados en la UI o respuestas,
    /// manteniendo consistencia en los valores mostrados.
    /// </summary>

    public static class DocumentGroupStatusExtensions
        {
          
            public static string ToLabel(this DocumentGroupStatus status)
            {
                return status switch
                {
                    DocumentGroupStatus.Pendiente => "Pendiente",
                    DocumentGroupStatus.Cargado => "Cargado",
                    DocumentGroupStatus.Observado => "Observado",
                    DocumentGroupStatus.Completo => "Completo",
                    _ => "Desconocido"
                };
            }
        
    }
}
