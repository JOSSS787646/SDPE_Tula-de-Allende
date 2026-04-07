using SistemaDigitalizacionPolizas.Domain.Entities.RequestStatusHistory_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IRequestStatusHistory
{
    /// <summary>
    /// Interfaz que define el repositorio para el registro del historial
    /// de cambios de estado de una solicitud.
    ///
    /// Permite almacenar cada transición de estado para fines de auditoría
    /// y trazabilidad del proceso.
    /// </summary>
    public interface IRequestStatusHistoryRepository
    {
        /// <summary>
        /// Registra un nuevo cambio de estado en el historial de la solicitud.
        /// </summary>
        Task AddAsync(RequestStatusHistory entity);
    }
}
