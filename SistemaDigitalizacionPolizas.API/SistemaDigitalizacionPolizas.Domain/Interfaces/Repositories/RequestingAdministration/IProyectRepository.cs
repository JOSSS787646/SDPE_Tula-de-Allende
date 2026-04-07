using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de proyectos.
    ///
    /// Permite realizar operaciones CRUD sobre los proyectos,
    /// incluyendo consultas por identificador y código, así como
    /// manejo de registros activos e inactivos.
    /// </summary>
    public interface IProyectRepository
    {
        /// <summary>
        /// Agrega un nuevo proyecto.
        /// </summary>
        Task<Proyect?> AddAsync(Proyect unit);

        /// <summary>
        /// Obtiene un proyecto mediante su código.
        /// </summary>
        Task<Proyect?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todos los proyectos registrados.
        /// </summary>
        Task<IEnumerable<Proyect>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un proyecto existente.
        /// </summary>
        Task<bool> UpdateAsync(Proyect unit);

        /// <summary>
        /// Realiza una eliminación lógica del proyecto,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int userId);

        /// <summary>
        /// Obtiene un proyecto por su código, incluyendo registros inactivos.
        /// </summary>
        Task<Proyect?> GetByCodeIncludingInactiveAsync(int code);

        /// <summary>
        /// Obtiene un proyecto por su identificador.
        /// </summary>
        Task<Proyect?> GetByIdAsync(int id);
    }
}