using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    /// <summary>
    /// Interfaz que define el servicio encargado de recalcular el estado
    /// de una solicitud.
    ///
    /// Evalúa las condiciones actuales (documentos, validaciones, etc.)
    /// para determinar y actualizar su estado correspondiente.
    /// </summary>
    public interface IRequestStatusService
    {
        /// <summary>
        /// Recalcula y actualiza el estado de la solicitud indicada.
        /// </summary>
        Task RecalculateStatus(int requestId);
    }
}
