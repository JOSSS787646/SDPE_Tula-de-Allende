using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document
{
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
        /// Persiste los cambios en base de datos.
        /// </summary>
        Task SaveChangesAsync();

        Task<RequestDocumentException?>
            GetByRequestAndDocumentTypeAsync(int requestId, int documentTypeId);


        Task<List<RequestDocumentException>> GetActiveByRequestId(int requestId);
    }
}
