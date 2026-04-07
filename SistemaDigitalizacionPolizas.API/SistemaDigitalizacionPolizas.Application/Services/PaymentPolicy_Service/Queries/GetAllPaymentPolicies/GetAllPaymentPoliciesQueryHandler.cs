using SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.GetAllPaymentPolicies
{
    public class GetPaymentPoliciesQueryHandler
    : IRequestHandler<GetPaymentPoliciesQuery, List<PaymentPolicyPreviewDto>>
    {
        private readonly IPaymentPolicyRepository _repository;
        private readonly IFileStorageService _fileStorage;

        public GetPaymentPoliciesQueryHandler(
            IPaymentPolicyRepository repository,
            IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task<List<PaymentPolicyPreviewDto>> Handle(
            GetPaymentPoliciesQuery request,
            CancellationToken cancellationToken)
        {
            var policies = await _repository.GetPagedAsync(
                request.Page,
                request.PageSize);

            return policies.Select(x => new PaymentPolicyPreviewDto
            {
                IdPaymentPolicy = x.IdPaymentPolicy,
                PolicyCode = x.PolicyCode,
                Description = x.Description,
                PreviewUrl = _fileStorage.GetPresignedUrl(x.FilePath)
            }).ToList();
        }
    }
}