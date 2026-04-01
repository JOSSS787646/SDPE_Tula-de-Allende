using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    /// <summary>
    /// Interfaz que define la política para determinar si se debe enviar
    /// una notificación asociada a una solicitud.
    ///
    /// Evalúa condiciones de negocio (como frecuencia, estado o reglas anti-spam)
    /// para evitar envíos innecesarios o duplicados.
    /// </summary>
    public interface INotificationPolicyService
    {
        /// <summary>
        /// Determina si es válido enviar una notificación para la solicitud indicada.
        /// 
        /// Retorna true si cumple las condiciones para envío, o false en caso contrario.
        /// </summary>
        Task<bool> ShouldSendNotificationAsync(int requestId);
    }
}
