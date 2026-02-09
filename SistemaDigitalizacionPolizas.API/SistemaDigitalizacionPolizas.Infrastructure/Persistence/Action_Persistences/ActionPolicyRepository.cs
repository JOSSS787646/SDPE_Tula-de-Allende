using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using Microsoft.EntityFrameworkCore;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Action_Persistences
{
    /// <summary>
    /// Repositorio de persistencia para la entidad ActionsPolicy.
    /// 
    /// Esta clase pertenece a la capa Infrastructure y es responsable
    /// únicamente de las operaciones de acceso a datos (CRUD).
    /// 
    /// No contiene reglas de negocio.
    /// </summary>
    public class ActionPolicyRepository : IActionPolicyRepository
    {
        private readonly SdpeDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// Inyecta el DbContext para acceso a base de datos.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework</param>
        public ActionPolicyRepository(SdpeDbContext context)
        {
            _context = context;
        }

        // ===============================
        // GET ALL
        // ===============================

        /// <summary>
        /// Obtiene todas las acciones registradas.
        /// 
        /// Se utiliza AsNoTracking porque es una operación de solo lectura
        /// y no se requiere seguimiento de cambios por Entity Framework.
        /// </summary>
        /// <returns>Listado de acciones</returns>
        public async Task<IEnumerable<ActionsPolicy>> GetAllAsync()
        {
            return await _context.Actions
                .AsNoTracking()
                .ToListAsync();
        }

        // ===============================
        // GET BY ID (Identificador técnico)
        // ===============================

        /// <summary>
        /// Obtiene una acción por su identificador técnico (IdAction).
        /// 
        /// El Id es inmutable y se utiliza para flujos internos
        /// como actualizaciones seguras.
        /// </summary>
        /// <param name="id">Id técnico de la acción</param>
        /// <returns>Acción encontrada o null</returns>
        public async Task<ActionsPolicy?> GetByIdAsync(int id)
        {
            return await _context.Actions
                .FirstOrDefaultAsync(x => x.IdAction == id);
        }

        // ===============================
        // GET BY CODE (Identificador de negocio)
        // ===============================

        /// <summary>
        /// Obtiene una acción por su código de negocio.
        /// 
        /// El Code es un identificador funcional definido por la empresa.
        /// Puede cambiar y no debe usarse como PK.
        /// </summary>
        /// <param name="code">Código de la acción</param>
        /// <returns>Acción encontrada o null</returns>
        public async Task<ActionsPolicy?> GetByCodeAsync(int code)
        {
            return await _context.Actions
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        // ===============================
        // ADD
        // ===============================

        /// <summary>
        /// Agrega una nueva acción al sistema.
        /// 
        /// Valida que no exista previamente una acción
        /// con el mismo código de negocio.
        /// </summary>
        /// <param name="unit">Entidad ActionsPolicy a crear</param>
        /// <returns>
        /// La entidad creada o null si el código ya existe
        /// </returns>
        public async Task<ActionsPolicy?> AddAsync(ActionsPolicy unit)
        {
            var exists = await _context.Actions
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.Actions.AddAsync(unit);
            await _context.SaveChangesAsync();

            return unit;
        }

        // ===============================
        // UPDATE
        // ===============================

        /// <summary>
        /// Actualiza una acción existente.
        /// 
        /// NOTA: Este método actualmente busca por Code,
        /// lo cual puede generar inconsistencias si el código
        /// cambia. Se recomienda actualizar por Id.
        /// </summary>
        /// <param name="unit">Entidad con los datos actualizados</param>
        /// <returns>true si se actualizó correctamente</returns>
        public async Task<bool> UpdateAsync(ActionsPolicy unit)
        {
            _context.Actions.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        // ===============================
        // DELETE (Soft Delete)
        // ===============================

        /// <summary>
        /// Realiza una eliminación lógica (soft delete) de una acción.
        /// 
        /// La acción no se elimina físicamente de la base de datos,
        /// solo se marca como inactiva.
        /// </summary>
        /// <param name="code">Código de la acción</param>
        /// <param name="userId">Usuario que realiza la eliminación</param>
        /// <returns>true si se eliminó correctamente</returns>
        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var action = await _context.Actions
                .FirstOrDefaultAsync(x => x.Code == code);

            if (action == null)
                return false;

            action.Active = false;
            action.UpdatedBy = userId;
            action.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
