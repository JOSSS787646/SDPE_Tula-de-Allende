using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    public class RequestDeleteService : IRequestDeleteService
    {
        private readonly IDocumentExpedientRepository _documentRepository;
        private readonly IAcquisitionRequest _requestRepository;
        private readonly IFileStorageService _fileService;

        public RequestDeleteService(
            IDocumentExpedientRepository documentRepository,
            IAcquisitionRequest requestRepository,
            IFileStorageService fileService)
        {
            _documentRepository = documentRepository;
            _requestRepository = requestRepository;
            _fileService = fileService;
        }

        public async Task DeleteRequestCascade(int requestId)
        {
            var documents = await _documentRepository.GetActiveByRequestId(requestId);

            foreach (var doc in documents)
            {
                if (!string.IsNullOrEmpty(doc.FilePath))
                {
                    await _fileService.DeleteFileAsync(doc.FilePath);
                }
            }

            await _requestRepository.DeleteCascadeAsync(requestId);
        }
    }
}
