using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface IUnitOfWorkService
    {
       
            Task BeginTransactionAsync();
            Task CommitAsync();
            Task RollbackAsync();
            Task SaveChangesAsync();
        
    }
}
