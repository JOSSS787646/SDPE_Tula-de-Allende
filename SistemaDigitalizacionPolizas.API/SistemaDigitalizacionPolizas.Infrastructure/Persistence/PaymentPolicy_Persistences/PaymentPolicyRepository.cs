using SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.PaymentPolicy_Persistences
{
    internal class PaymentPolicyRepository: IPaymentPolicyRepository
    {

        private readonly SdpeDbContext _context;

        public PaymentPolicyRepository(SdpeDbContext context)
        {
            _context = context;
        }


        public async Task<PaymentPolicy> CreateAsync(PaymentPolicy policy)
        {
            await _context.PaymentPolicies.AddAsync(policy);
            return policy;
        }



        public async Task DeleteAsync(int id)
        {
            var policy = await _context.PaymentPolicies
                .FirstOrDefaultAsync(x => x.IdPaymentPolicy == id);

            if (policy == null)
                throw new Exception("Policy not found");

            _context.PaymentPolicies.Remove(policy);

            await _context.SaveChangesAsync();
        }


        public async Task<List<PaymentPolicy>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.PaymentPolicies
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<List<PaymentPolicy>> GetAvailablePoliciesAsync()
        {
            return await _context.PaymentPolicies
                .Where(p => p.IsActive &&
                       !_context.AcquisitionRequests
                           .Any(s => s.IdPaymentPolicy == p.IdPaymentPolicy))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaymentPolicy?> GetByIdAsync(int id)
        {
            return await _context.PaymentPolicies
                .FirstOrDefaultAsync(x => x.IdPaymentPolicy == id);
        }

        public async Task<PaymentPolicy?> GetByPolicyCodeAsync(string policyCode)
        {
            return await _context.PaymentPolicies
                .FirstOrDefaultAsync(x => x.PolicyCode == policyCode);
        }

        public async Task UpdateAsync(PaymentPolicy policy)
        {
            _context.PaymentPolicies.Update(policy);
        }
    }
}
