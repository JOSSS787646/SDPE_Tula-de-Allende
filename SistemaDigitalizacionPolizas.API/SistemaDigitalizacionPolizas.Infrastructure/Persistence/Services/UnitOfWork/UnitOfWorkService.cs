using Microsoft.EntityFrameworkCore.Storage;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UnitOfWork
{
    public class UnitOfWorkService: IUnitOfWorkService
    {
        private readonly SdpeDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWorkService(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }


  

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay transacción activa.");

            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();
            _transaction = null; // 🔥 IMPORTANTE
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return; // 🔥 NO INTENTAR ROLLBACK SI YA MURIÓ

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null; // 🔥 IMPORTANTE
        }
    }
}
