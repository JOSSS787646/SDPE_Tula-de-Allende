using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de tipos de documento.
    ///
    /// Permite realizar operaciones CRUD sobre los tipos de documento,
    /// incluyendo consultas por nombre, identificador y paginación.
    /// </summary>
    public interface IDocumentTypeRepository
    {
        /// <summary>
        /// Agrega un nuevo tipo de documento.
        /// </summary>
        Task<DocumentType?> AddAsync(DocumentType unit);

        /// <summary>
        /// Obtiene un tipo de documento mediante su nombre.
        /// </summary>
        Task<DocumentType?> GetByNameAsync(string documentName);

        /// <summary>
        /// Obtiene tipos de documento de forma paginada.
        /// </summary>
        Task<IEnumerable<DocumentType>> GetPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Actualiza la información de un tipo de documento existente.
        /// </summary>
        Task<bool> UpdateAsync(DocumentType unit);

        /// <summary>
        /// Realiza una eliminación lógica del tipo de documento.
        /// </summary>
        Task<bool> DeleteAsync(string name);

        /// <summary>
        /// Obtiene un tipo de documento por su identificador.
        /// </summary>
        Task<DocumentType?> GetByIdAsync(int id);
    }
}
