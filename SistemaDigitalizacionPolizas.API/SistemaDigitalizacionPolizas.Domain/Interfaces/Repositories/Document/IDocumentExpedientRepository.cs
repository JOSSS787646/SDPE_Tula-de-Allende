using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de documentos
    /// del expediente asociados a solicitudes.
    ///
    /// Permite realizar operaciones de creación, consulta, actualización,
    /// eliminación y búsqueda de documentos, así como obtener información
    /// específica para checklist, paginación y visualización.
    /// </summary>
    public interface IDocumentExpedientRepository
    {
        /// <summary>
        /// Agrega un nuevo documento al expediente.
        /// </summary>
        Task<ExpedientDocument> AddAsync(ExpedientDocument entity);

        /// <summary>
        /// Persiste los cambios pendientes en el contexto.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Obtiene documentos por clasificación con paginación,
        /// retornando los elementos y el total de registros.
        /// </summary>
        Task<(List<ExpedientDocument> Items, int Total)>
            GetByClassificationAsync(int classificationId, int page, int pageSize);

        /// <summary>
        /// Obtiene el checklist de documentos asociados a una solicitud.
        /// </summary>
        Task<List<RequestDocumentChecklistDto>> GetChecklistByRequestAsync(int requestId);

        /// <summary>
        /// Agrega múltiples documentos en una sola operación.
        /// </summary>
        Task AddRangeAsync(List<ExpedientDocument> entities);

        /// <summary>
        /// Obtiene un documento por su identificador.
        /// </summary>
        Task<ExpedientDocument?> GetByIdAsync(int id);

        /// <summary>
        /// Marca un documento para actualización en el contexto.
        /// </summary>
        void Update(ExpedientDocument entity);

        /// <summary>
        /// Busca documentos por nombre dentro de una solicitud.
        /// </summary>
        Task<List<ExpedientDocumentSearchDto>>
            SearchByNameAsync(int requestId, string fileName);

        /// <summary>
        /// Obtiene todos los documentos activos asociados a una solicitud.
        /// </summary>
        Task<List<ExpedientDocument>> GetActiveByRequestId(int requestId);

        /// <summary>
        /// Obtiene documentos por clasificación para vista previa.
        /// </summary>
        Task<IEnumerable<ExpedientDocumentPreviewDto>> GetDocumentsByClassificationAsync(int classificationId);

        /// <summary>
        /// Marca un documento para eliminación en el contexto.
        /// </summary>
        void Delete(ExpedientDocument entity);

        /// <summary>
        /// Actualiza un documento de forma persistente.
        /// </summary>
        Task UpdateAsync(ExpedientDocument entity);
    }
}
