using Microsoft.EntityFrameworkCore;
using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.NotificationRepository_Persistences
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SdpeDbContext _context;

        public NotificationRepository(SdpeDbContext context)
        {
            _context = context;
        }

        // 🔔 SOLO NO LEÍDAS (para campanita)
        public async Task<List<Notification>> GetUnreadByUserAsync(int userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead && x.Active)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // 📚 TODAS (historial)
        public async Task<List<Notification>> GetAllByUserAsync(int userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId && x.Active)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // 📄 Obtener por Id
        public async Task<Notification?> GetByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(x => x.IdNotification == notificationId && x.Active);
        }

        // 🔥 PRO: Obtener y marcar como leída
        public async Task<Notification?> GetAndMarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.IdNotification == notificationId && x.Active);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return notification;
        }

        // ➕ Crear
        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        // ✔ Marcar una como leída
        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.IdNotification == notificationId && x.Active);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        // ✔ Marcar todas como leídas
        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead && x.Active)
                .ToListAsync();

            if (notifications.Any())
            {
                foreach (var n in notifications)
                    n.IsRead = true;

                await _context.SaveChangesAsync();
            }
        }

        // 🔢 Contador
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(x => x.UserId == userId && !x.IsRead && x.Active);
        }

        // 📑 Preview de documentos
        public async Task<DocumentPreviewInfoDto?> GetDocumentsUploadedPreviewAsync(int requestId)
        {
            var request = await _context.AcquisitionRequests
                .Include(r => r.AdministrativeUnit)
                .FirstOrDefaultAsync(r => r.IdRequest == requestId);

            if (request == null) return null;

            var documents = await _context.ExpedientDocuments
                .Where(d => d.RequestId == requestId)
                .Include(d => d.DocumentStatus)
                .Select(d => new DocumentItemDto
                {
                    FileName = d.FileName ?? "Sin nombre",
                    Status = d.DocumentStatus != null
                        ? d.DocumentStatus.Description
                        : "Sin estado",
                    Observations = d.ObservationsUpload
                })
                .ToListAsync();

            return new DocumentPreviewInfoDto
            {
                RequestNumber = request.RequestNumber ?? "",
                AdministrativeUnit = request.AdministrativeUnit.Description,
                Description = request.Justification ?? "",
                Date = DateTime.Now,
                Documents = documents
            };





        }

        // -- Elimanr el registro
        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.IdNotification == notificationId);

            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return true;
        }

        // -- Eliminar todas las notificaciones de un usuario
        public async Task DeleteAllByUserAsync(int userId)
        {
            await _context.Notifications
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync();
        }

    }
}