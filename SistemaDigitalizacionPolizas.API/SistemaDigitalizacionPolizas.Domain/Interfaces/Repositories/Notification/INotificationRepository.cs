using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUnreadByUserAsync(int userId);

        // 📚 Historial (todas)
        Task<List<Notification>> GetAllByUserAsync(int userId);

        // 📄 Obtener por Id
        Task<Notification?> GetByIdAsync(int notificationId);

        // 📄 Obtener detalle y marcar como leída (PRO)
        Task<Notification?> GetAndMarkAsReadAsync(int notificationId);

        // ➕ Crear notificación
        Task AddAsync(Notification notification);

        // ✔ Marcar como leída
        Task MarkAsReadAsync(int notificationId);

        // ✔ Marcar todas como leídas
        Task MarkAllAsReadAsync(int userId);

        // 🔢 Contador
        Task<int> GetUnreadCountAsync(int userId);

        // 📑 Preview de documentos (tu caso)
        Task<DocumentPreviewInfoDto?> GetDocumentsUploadedPreviewAsync(int requestId);

        //Elimar una notificaion 
        Task<bool> DeleteAsync(int notificationId);

        // Eliminar todas las notificaciones de un usuario
        Task DeleteAllByUserAsync(int userId);
    }
}
