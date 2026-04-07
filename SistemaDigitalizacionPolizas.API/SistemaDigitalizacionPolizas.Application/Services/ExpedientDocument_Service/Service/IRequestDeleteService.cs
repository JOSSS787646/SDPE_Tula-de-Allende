using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    /// <summary>
    /// Interfaz que define el servicio para eliminar una solicitud junto con
    /// todas sus entidades relacionadas (eliminación en cascada).
    ///
    /// Se encarga de garantizar la integridad de los datos al remover
    /// dependencias asociadas a la solicitud.
    /// </summary>
    public interface IRequestDeleteService
    {
        /// <summary>
        /// Elimina una solicitud y todos sus datos relacionados de forma controlada.
        /// </summary>
        Task DeleteRequestCascade(int requestId);
    }
}
