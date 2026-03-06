using SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy
{
    public interface IPaymentPolicyRepository
    {
        Task<PaymentPolicy> CreateAsync(PaymentPolicy policy);

        Task<PaymentPolicy?> GetByIdAsync(int id);

        Task<PaymentPolicy?> GetByPolicyCodeAsync(string policyCode);
        Task<List<PaymentPolicy>> GetPagedAsync(int page, int pageSize)

        Task UpdateAsync(PaymentPolicy policy);
    }
}
