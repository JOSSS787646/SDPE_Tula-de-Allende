using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers
{
    public interface ISupplierRepository
    {
        Task<Supplier> AddAsync(Supplier unit);

        Task<bool> ExistsByRfcAsync(string rfc);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<bool> UpdateAsync(Supplier unit);
        Task<bool> DeleteAsync(string rfc, int supplierid);
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier?> GetByRfcAsync(string rfc);
        Task<bool> ExistsByPhoneAsync(string phone);
        Task<List<Supplier>> GetPagedAsync(int page, int pageSize);

    }
}
