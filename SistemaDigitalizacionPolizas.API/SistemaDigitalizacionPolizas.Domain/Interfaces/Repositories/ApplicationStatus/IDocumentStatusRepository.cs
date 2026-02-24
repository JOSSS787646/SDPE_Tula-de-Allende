using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest
{
    public interface IDocumentStatusRepository
    {

        //Agregar un nuevo fondo
        Task<DocumentStatus?> AddAsync(DocumentStatus unit);
        //Obtener un fondo por su codigo
        Task<DocumentStatus?> GetByCodeAsync(int code);
        //Obtener todos los fondos
        Task<IEnumerable<DocumentStatus>> GetAllAsync();
        //Acctualizar la informacion de un fondo
        Task<bool> UpdateAsync(DocumentStatus unit);
        //Desactivar un fondo
        Task<bool> DeleteAsync(int code, int userId);
        Task<DocumentStatus?> GetByIdAsync(int id);
    }
}
