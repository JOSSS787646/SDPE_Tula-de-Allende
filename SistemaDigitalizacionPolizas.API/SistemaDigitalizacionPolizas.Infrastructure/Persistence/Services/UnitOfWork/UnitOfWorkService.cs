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

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
