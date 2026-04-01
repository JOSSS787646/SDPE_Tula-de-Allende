using Microsoft.EntityFrameworkCore.Storage;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Implementación del patrón Unit of Work para el manejo de transacciones.
///
/// Permite agrupar múltiples operaciones sobre la base de datos dentro
/// de una misma transacción, asegurando consistencia mediante commit
/// o rollback.
///
/// Controla el ciclo de vida de la transacción y la persistencia de cambios
/// usando el DbContext de Entity Framework.
/// </summary>
/// 



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
            _transaction = null; 
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null; 
        }
    }
}
