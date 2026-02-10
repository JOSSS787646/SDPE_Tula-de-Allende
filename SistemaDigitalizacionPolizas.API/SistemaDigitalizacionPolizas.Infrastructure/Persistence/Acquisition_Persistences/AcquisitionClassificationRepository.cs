using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Acquisition_Persistences
{
    /// <summary>
    /// Repositorio para la gestión de la Clasificación de Adquisiciones.
    /// </summary>
    public class AcquisitionClassificationRepository : IAcquisitionClassificationRepository
    {
        private readonly SdpeDbContext _context;

        public AcquisitionClassificationRepository(SdpeDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las clasificaciones de adquisiciones.
        /// </summary>
        public async Task<IEnumerable<AcquisitionClassification>> GetAllAsync()
        {
            return await _context.AcquisitionClassifications
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una clasificación de adquisición por su código.
        /// </summary>
        public async Task<AcquisitionClassification?> GetByCodeAsync(int code)
        {
            return await _context.AcquisitionClassifications
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        /// <summary>
        /// Obtiene una clasificación de adquisición por su Id.
        /// </summary>
        public async Task<AcquisitionClassification?> GetByIdAsync(int idAcquisitionClassification)
        {
            return await _context.AcquisitionClassifications
                .FirstOrDefaultAsync(x => x.idAcquisitionClassification == idAcquisitionClassification);
        }

        /// <summary>
        /// Agrega una nueva clasificación de adquisición.
        /// </summary>
        public async Task<AcquisitionClassification?> AddAsync(AcquisitionClassification unit)
        {
            var exists = await _context.AcquisitionClassifications
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.AcquisitionClassifications.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }

        /// <summary>
        /// Actualiza una clasificación de adquisición existente.
        /// </summary>
        public async Task<bool> UpdateAsync(AcquisitionClassification unit)
        {
            _context.AcquisitionClassifications.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Desactiva una clasificación de adquisición (eliminación lógica).
        /// </summary>
        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var cog = await _context.AcquisitionClassifications
                .FirstOrDefaultAsync(x => x.Code == code);

            if (cog == null)
                return false;

            cog.Active = false;
            cog.UpdatedBy = userId;
            cog.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
