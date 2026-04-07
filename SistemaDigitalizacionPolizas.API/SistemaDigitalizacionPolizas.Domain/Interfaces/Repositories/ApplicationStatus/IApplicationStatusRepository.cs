using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de estados de aplicación.
    ///
    /// Permite realizar operaciones CRUD sobre los estados utilizados en las solicitudes,
    /// incluyendo consultas por Id y código, así como eliminación lógica.
    /// </summary>
    public interface IApplicationStatusRepository
    {
        /// <summary>
        /// Agrega un nuevo estado de aplicación.
        /// </summary>
        Task<ApplicationStatus?> AddAsync(ApplicationStatus unit);

        /// <summary>
        /// Obtiene un estado de aplicación mediante su código.
        /// </summary>
        Task<ApplicationStatus?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todos los estados de aplicación registrados.
        /// </summary>
        Task<IEnumerable<ApplicationStatus>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un estado de aplicación existente.
        /// </summary>
        Task<bool> UpdateAsync(ApplicationStatus unit);

        /// <summary>
        /// Realiza una eliminación lógica del estado de aplicación,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int userId);

        /// <summary>
        /// Obtiene un estado de aplicación por su identificador.
        /// </summary>
        Task<ApplicationStatus?> GetByIdAsync(int id);
    }
}
