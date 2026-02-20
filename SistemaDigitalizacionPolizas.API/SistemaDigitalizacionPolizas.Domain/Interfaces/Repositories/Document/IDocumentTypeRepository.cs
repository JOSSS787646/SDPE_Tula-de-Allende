using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document
{
    public interface IDocumentTypeRepository
    {
        Task<DocumentType?> AddAsync(DocumentType unit);
        //Obtener un fondo por su codigo
        Task<DocumentType?> GetByNameAsync(string documentName);
        //Obtener todos los fondos
        Task<IEnumerable<DocumentType>> GetPagedAsync(int pageNumber, int pageSize);
        //Acctualizar la informacion de un fondo
        Task<bool> UpdateAsync(DocumentType unit);
        //Desactivar un fondo
        Task<bool> DeleteAsync(string name);
        Task<DocumentType?> GetByIdAsync(int id);
    }
}
