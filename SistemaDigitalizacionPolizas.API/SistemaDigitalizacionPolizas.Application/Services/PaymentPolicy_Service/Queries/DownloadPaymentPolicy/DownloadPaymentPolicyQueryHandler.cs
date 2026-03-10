using SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.DownloadPaymentPolicy
{
    public class DownloadPaymentPolicyQueryHandler
        : IRequestHandler<DownloadPaymentPolicyQuery, DownloadPaymentPolicyDto>
    {
        private readonly IPaymentPolicyRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public DownloadPaymentPolicyQueryHandler(
            IPaymentPolicyRepository repository,
            IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<DownloadPaymentPolicyDto> Handle(
            DownloadPaymentPolicyQuery request,
            CancellationToken cancellationToken)
        {
            var policy = await _repository.GetByIdAsync(request.PaymentPolicyId);

            if (policy == null || string.IsNullOrEmpty(policy.FilePath))
                throw new Exception("Póliza no encontrada");

            var file = await _fileStorageService.GetFileAsync(policy.FilePath);

            var fileName = Path.GetFileName(policy.FilePath);

            return new DownloadPaymentPolicyDto(
                fileName,
                file.Headers.ContentType,
                file.ResponseStream
            );
        }
    }
}
