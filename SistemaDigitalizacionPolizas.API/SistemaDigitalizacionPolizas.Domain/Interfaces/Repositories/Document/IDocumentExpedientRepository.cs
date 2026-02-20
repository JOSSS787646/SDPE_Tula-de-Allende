using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document
{
    public interface IDocumentExpedientRepository
    {
        Task<ExpedientDocument> AddAsync(ExpedientDocument entity);

        Task SaveChangesAsync();
    }
}
