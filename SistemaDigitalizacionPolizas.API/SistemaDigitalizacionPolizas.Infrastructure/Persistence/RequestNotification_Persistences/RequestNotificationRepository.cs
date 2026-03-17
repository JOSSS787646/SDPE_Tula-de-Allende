using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Enums;
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
                    RequestDate = r.RequestDate, // ← IMPORTANTE
                    AdministrativeUnitName = r.AdministrativeUnit.Description,
                    Justification = r.Justification
                })
                .FirstOrDefaultAsync();
        }


        public async Task<DocumentApprovedNotificationDto?> GetDocumentApprovedInfoAsync(int requestId)
        {
            return await _context.AcquisitionRequests
                .Where(r => r.IdRequest == requestId)
                .Select(r => new DocumentApprovedNotificationDto
                {
                    RequestNumber = r.RequestNumber,

                    ManagerEmail = _context.RequestManagers
                        .Where(m => m.IdRequest == r.IdRequest)
                        .Select(m => m.Email)
                        .FirstOrDefault(),

                    ManagerFullName = _context.RequestManagers
                        .Where(m => m.IdRequest == r.IdRequest)
                        .Select(m => m.FirstName + " " + m.LastName)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        }


        public async Task<DocumentObservedNotificationDto?> GetDocumentObservedInfoAsync(int requestId)
        {
            return await _context.AcquisitionRequests
                .Where(r => r.IdRequest == requestId)
                .Select(r => new DocumentObservedNotificationDto
                {
                    RequestNumber = r.RequestNumber,

                    ManagerEmail = _context.RequestManagers
                        .Where(m => m.IdRequest == r.IdRequest)
                        .Select(m => m.Email)
                        .FirstOrDefault(),

                    ManagerFullName = _context.RequestManagers
                        .Where(m => m.IdRequest == r.IdRequest)
                        .Select(m => m.FirstName + " " + m.LastName)
                        .FirstOrDefault(),

                    AdminEmail = _context.Users
                        .Where(u => u.IdRole == (int)SystemRolesEnum.AdministradorAdquisiciones)
                        .Select(u => u.Email)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        }
    }
}
