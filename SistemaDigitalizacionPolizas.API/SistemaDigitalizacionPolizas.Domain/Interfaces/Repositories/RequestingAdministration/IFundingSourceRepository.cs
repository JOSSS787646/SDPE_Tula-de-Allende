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
    /// Interfaz que define el repositorio para la gestión de fuentes de financiamiento.
    ///
    /// Permite realizar operaciones CRUD sobre los fondos, incluyendo
    /// consultas por identificador y código, así como eliminación lógica.
    /// </summary>
    public interface IFundingSourceRepository
    {
        /// <summary>
        /// Agrega una nueva fuente de financiamiento.
        /// </summary>
        Task<FundingSource?> AddAsync(FundingSource unit);

        /// <summary>
        /// Obtiene una fuente de financiamiento mediante su código.
        /// </summary>
        Task<FundingSource?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todas las fuentes de financiamiento registradas.
        /// </summary>
        Task<IEnumerable<FundingSource>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de una fuente de financiamiento existente.
        /// </summary>
        Task<bool> UpdateAsync(FundingSource unit);

        /// <summary>
        /// Realiza una eliminación lógica de la fuente de financiamiento.
        /// </summary>
        Task<bool> DeleteAsync(int code);

        /// <summary>
        /// Obtiene una fuente de financiamiento por su identificador.
        /// </summary>
        Task<FundingSource?> GetByIdAsync(int id);
    }
}
