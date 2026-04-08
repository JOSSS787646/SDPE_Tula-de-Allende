using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de la relación
    /// entre clasificaciones de adquisición y tipos de documento.
    ///
    /// Permite administrar los documentos requeridos por cada clasificación,
    /// incluyendo operaciones de carga masiva, consulta, validación y eliminación.
    /// </summary>
    public interface IClasificationDocumentTypeRepository
    {
        /// <summary>
        /// Reemplaza todos los documentos asociados a una clasificación,
        /// realizando una carga masiva de relaciones.
        /// </summary>
        Task UpsertRangeAsync(
      int acquisitionClassificationId,
      IEnumerable<ClasificationDocumentType> entities);

        /// <summary>
        /// Obtiene todos los documentos asociados a una clasificación.
        /// </summary>
        Task<List<ClasificationDocumentType>>
            GetByClassificationAsync(int acquisitionClassificationId);

        /// <summary>
        /// Elimina todas las relaciones de documentos asociadas a una clasificación.
        /// </summary>
        Task RemoveByClassificationAsync(int acquisitionClassificationId);

        /// <summary>
        /// Verifica si existe una relación entre una clasificación
        /// y un tipo de documento específico.
        /// </summary>
        Task<bool> ExistsAsync(
            int acquisitionClassificationId,
            int documentTypeId);

        /// <summary>
        /// Obtiene los documentos requeridos para una clasificación específica.
        /// </summary>
        Task<List<ClasificationDocumentType>> GetRequiredByClassification(int classificationId);
    }
}
