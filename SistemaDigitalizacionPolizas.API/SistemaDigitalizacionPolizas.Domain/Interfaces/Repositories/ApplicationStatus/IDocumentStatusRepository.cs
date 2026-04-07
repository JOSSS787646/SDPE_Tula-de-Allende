using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de estados de documentos.
    ///
    /// Permite realizar operaciones CRUD sobre los estados de los documentos,
    /// incluyendo consultas por Id y código, así como eliminación lógica.
    /// </summary>
    public interface IDocumentStatusRepository
    {
        /// <summary>
        /// Agrega un nuevo estado de documento.
        /// </summary>
        Task<DocumentStatus?> AddAsync(DocumentStatus unit);

        /// <summary>
        /// Obtiene un estado de documento mediante su código.
        /// </summary>
        Task<DocumentStatus?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todos los estados de documento registrados.
        /// </summary>
        Task<IEnumerable<DocumentStatus>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un estado de documento existente.
        /// </summary>
        Task<bool> UpdateAsync(DocumentStatus unit);

        /// <summary>
        /// Realiza una eliminación lógica del estado de documento,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int userId);

        /// <summary>
        /// Obtiene un estado de documento por su identificador.
        /// </summary>
        Task<DocumentStatus?> GetByIdAsync(int id);
    }
}
