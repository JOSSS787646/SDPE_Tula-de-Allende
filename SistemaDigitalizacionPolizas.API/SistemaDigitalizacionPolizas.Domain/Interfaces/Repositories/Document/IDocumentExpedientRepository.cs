using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
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


        Task<(List<ExpedientDocument> Items, int Total)>
             GetByClassificationAsync(int classificationId, int page, int pageSize);

        Task<List<RequestDocumentChecklistDto>> GetChecklistByRequestAsync(int requestId);

    }
}
