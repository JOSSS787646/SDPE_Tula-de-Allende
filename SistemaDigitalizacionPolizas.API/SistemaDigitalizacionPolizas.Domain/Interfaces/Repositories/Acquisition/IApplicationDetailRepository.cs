using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de detalles
    /// asociados a una solicitud.
    ///
    /// Permite consultar, crear, actualizar, eliminar y sincronizar
    /// múltiples detalles vinculados a una solicitud específica.
    /// </summary>
    public interface IApplicationDetailRepository
    {
        /// <summary>
        /// Obtiene todos los detalles de una solicitud.
        /// </summary>
        Task<List<ApplicationDetail>> GetByRequestIdAsync(int requestId);

        /// <summary>
        /// Obtiene un detalle por su identificador.
        /// </summary>
        Task<ApplicationDetail?> GetByIdAsync(int detailId);

        /// <summary>
        /// Agrega un nuevo detalle y retorna su Id generado.
        /// </summary>
        Task<int> AddAsync(ApplicationDetail detail);

        /// <summary>
        /// Actualiza un detalle existente.
        /// </summary>
        Task UpdateAsync(ApplicationDetail detail);

        /// <summary>
        /// Elimina un detalle por su Id.
        /// </summary>
        Task DeleteAsync(int detailId);

        /// <summary>
        /// Inserta o actualiza múltiples detalles para una solicitud,
        /// sincronizando los registros en una sola operación.
        /// </summary>
        Task UpsertRangeAsync(int requestId, List<ApplicationDetail> details);
    }
}
