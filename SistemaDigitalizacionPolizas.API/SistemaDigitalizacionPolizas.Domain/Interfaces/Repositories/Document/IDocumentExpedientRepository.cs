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
    public interface IDocumentExpedientRepository
    {
        Task<ExpedientDocument> AddAsync(ExpedientDocument entity);

        Task SaveChangesAsync();


        Task<(List<ExpedientDocument> Items, int Total)>
             GetByClassificationAsync(int classificationId, int page, int pageSize);

        Task<List<RequestDocumentChecklistDto>> GetChecklistByRequestAsync(int requestId);


        Task AddRangeAsync(List<ExpedientDocument> entities);

        Task<ExpedientDocument?> GetByIdAsync(int id);
        void Update(ExpedientDocument entity);

        Task<List<ExpedientDocumentSearchDto>>
    SearchByNameAsync(int requestId, string fileName);


        Task<List<ExpedientDocument>> GetActiveByRequestId(int requestId);

        Task<IEnumerable<ExpedientDocumentPreviewDto>> GetDocumentsByClassificationAsync(int classificationId);
        void Delete(ExpedientDocument entity);

    }
}
