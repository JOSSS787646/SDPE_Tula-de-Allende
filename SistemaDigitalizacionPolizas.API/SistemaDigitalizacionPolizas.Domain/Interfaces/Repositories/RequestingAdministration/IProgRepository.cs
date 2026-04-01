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
    /// Interfaz que define el repositorio para la gestión de programas (Prog).
    ///
    /// Permite realizar operaciones CRUD sobre los programas,
    /// incluyendo consultas por identificador y código, así como eliminación lógica.
    /// </summary>
    public interface IProgRepository
    {
        /// <summary>
        /// Agrega un nuevo programa.
        /// </summary>
        Task<Prog?> AddAsync(Prog unit);

        /// <summary>
        /// Obtiene un programa mediante su código.
        /// </summary>
        Task<Prog?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todos los programas registrados.
        /// </summary>
        Task<IEnumerable<Prog>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un programa existente.
        /// </summary>
        Task<bool> UpdateAsync(Prog unit);

        /// <summary>
        /// Realiza una eliminación lógica del programa,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int userId);

        /// <summary>
        /// Obtiene un programa por su identificador.
        /// </summary>
        Task<Prog?> GetByIdAsync(int id);
    }
}
