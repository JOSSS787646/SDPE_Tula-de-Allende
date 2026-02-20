using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    public interface IClasificationDocumentTypeRepository
    {
        /// <summary>
        /// Reemplaza todos los documentos asociados
        /// a una clasificación (carga masiva).
        /// </summary>
        Task ReplaceAsync(
            int acquisitionClassificationId,
            IEnumerable<ClasificationDocumentType> entities);

        /// <summary>
        /// Obtiene todos los documentos de una clasificación.
        /// </summary>
        Task<List<ClasificationDocumentType>>
            GetByClassificationAsync(int acquisitionClassificationId);

        /// <summary>
        /// Elimina todas las relaciones de una clasificación.
        /// </summary>
        Task RemoveByClassificationAsync(int acquisitionClassificationId);

        /// <summary>
        /// Verifica si existe una relación específica.
        /// </summary>
        Task<bool> ExistsAsync(
            int acquisitionClassificationId,
            int documentTypeId);

    }
}
