using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de excepciones
    /// de documentos en una solicitud.
    ///
    /// Permite registrar, actualizar y consultar excepciones que indican
    /// que ciertos documentos no aplican o tienen condiciones especiales
    /// dentro de una solicitud.
    /// </summary>
    public interface IRequestDocumentExceptionRepository
    {
        /// <summary>
        /// Agrega una nueva excepción de documento.
        /// </summary>
        Task AddAsync(RequestDocumentException entity);

        /// <summary>
        /// Actualiza una excepción existente.
        /// </summary>
        Task UpdateAsync(RequestDocumentException entity);

        /// <summary>
        /// Persiste los cambios pendientes en la base de datos.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Obtiene una excepción específica para una solicitud y tipo de documento.
        /// </summary>
        Task<RequestDocumentException?>
            GetByRequestAndDocumentTypeAsync(int requestId, int documentTypeId);

        /// <summary>
        /// Obtiene todas las excepciones activas de una solicitud.
        /// </summary>
        Task<List<RequestDocumentException>> GetActiveByRequestId(int requestId);

        /// <summary>
        /// Inserta o actualiza una excepción (upsert).
        /// </summary>
        Task UpsertAsync(RequestDocumentException entity);

        /// <summary>
        /// Inserta o actualiza múltiples excepciones en una sola operación.
        /// </summary>
        Task UpsertRangeAsync(List<RequestDocumentException> entities);
    }
}
