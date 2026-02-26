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
        Task<RequestDocumentException?> GetByIdAsync(int id);

        Task<IEnumerable<RequestDocumentException>> GetByRequestIdAsync(int requestId);

        Task AddAsync(RequestDocumentException entity);

        Task UpdateAsync(RequestDocumentException entity);

        Task DeleteAsync(int id); // Soft delete

        Task<bool> ExistsAsync(int requestId, int documentTypeId);

        Task SaveChangesAsync();
    }
}
