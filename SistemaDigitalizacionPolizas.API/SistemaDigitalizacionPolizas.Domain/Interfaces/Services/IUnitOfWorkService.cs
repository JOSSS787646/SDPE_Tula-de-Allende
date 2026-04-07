using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    /// <summary>
    /// Interfaz que define el patrón Unit of Work para la gestión de transacciones.
    ///
    /// Permite agrupar múltiples operaciones en una sola transacción,
    /// garantizando consistencia en la base de datos mediante control
    /// de confirmación (commit) y reversión (rollback).
    /// </summary>
    public interface IUnitOfWorkService
    {
        /// <summary>
        /// Inicia una nueva transacción.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual, persistiendo los cambios.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Revierte la transacción actual en caso de error.
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Guarda los cambios pendientes en el contexto sin confirmar la transacción.
        /// </summary>
        Task SaveChangesAsync();
    }
}

