using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestNotification_Persistences
{
    public class RequestNotificationRepository : IRequestNotificationRepository
    {

        private readonly SdpeDbContext _context;

        public RequestNotificationRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<RequestNotificationDto?> GetRequestNotificationInfoAsync(int requestId)
        {
            return await _context.AcquisitionRequests
                .Where(r => r.IdRequest == requestId)
                .Select(r => new RequestNotificationDto
                {
                    RequestNumber = r.RequestNumber,
                    AdministrativeUnitName = r.AdministrativeUnit.Description, 
                    Justification = r.Justification 
                })
                .FirstOrDefaultAsync();
        }
    }
}
