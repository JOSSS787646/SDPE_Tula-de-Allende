using Microsoft.EntityFrameworkCore;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestManager_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestManager_Persistences
{
    public class RequestManagerRepository : IRequestManagerRepository
    {
        private readonly SdpeDbContext _context;

        public RequestManagerRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RequestManagerPreviewDto>> GetAllAsync()
        {
            return await (
                from rm in _context.RequestManagers

                join r in _context.AcquisitionRequests
                    on rm.IdRequest equals r.IdRequest

                join ua in _context.AdministrativeUnits
                    on rm.IdAdministrativeUnit equals ua.IdAdministrativeUnit into uaJoin
                from ua in uaJoin.DefaultIfEmpty()

                select new RequestManagerPreviewDto
                {
                    IdRequestManager = rm.IdRequestManager,

                    FullName = rm.FirstName + " " + rm.LastName + " " + (rm.SecondLastName ?? ""),

                    RequestNumber = r.RequestNumber,

                    Email = rm.Email,

                    AdministrativeUnit = ua != null ? ua.Description : null
                }

            ).ToListAsync();
        }

        public async Task<int> AddAsync(CreateRequestManagerDto dto)
        {
            var entity = new RequestManager
            {
                IdRequest = dto.IdRequest,
                IdAdministrativeUnit = dto.IdAdministrativeUnit,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                SecondLastName = dto.SecondLastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.RequestManagers.Add(entity);

            await _context.SaveChangesAsync();

            return entity.IdRequestManager;
        }

        public async Task<bool> UpdateAsync(UpdateRequestManagerDto dto)
        {
            var entity = await _context.RequestManagers
                .FirstOrDefaultAsync(x => x.IdRequestManager == dto.IdRequestManager);

            if (entity == null)
                return false;

            entity.IdAdministrativeUnit = dto.IdAdministrativeUnit;
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.SecondLastName = dto.SecondLastName;
            entity.Email = dto.Email;
            entity.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}