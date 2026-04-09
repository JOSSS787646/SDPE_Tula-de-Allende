using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de solicitudes de adquisición.
    ///
    /// Proporciona operaciones para crear, consultar, actualizar y eliminar solicitudes,
    /// así como métodos especializados para manejo de estados, paginación,
    /// notificaciones y lógica de negocio asociada.
    /// </summary>
    public interface IAcquisitionRequest
    {
        /// <summary>
        /// Crea una nueva solicitud de adquisición.
        /// </summary>
        Task<AcquisitionRequest?> AddAsync(AcquisitionRequest request);

        /// <summary>
        /// Obtiene el detalle completo de una solicitud por Id.
        /// </summary>
        Task<AcquisitionRequestDetailDto?> GetDetailAsync(int idRequest);

        /// <summary>
        /// Obtiene una solicitud por su identificador.
        /// </summary>
        Task<AcquisitionRequest?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene información paginada de solicitudes tipo póliza,
        /// incluyendo el total de registros.
        /// </summary>
        Task<(IEnumerable<AcquisitionRequestPolizaDto> Data, int TotalRecords)>
            GetAllPolizaInfoPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Actualiza una solicitud existente.
        /// </summary>
        Task UpdateAsync(AcquisitionRequest entity);

        /// <summary>
        /// Elimina una solicitud junto con sus entidades relacionadas
        /// (eliminación en cascada).
        /// </summary>
        Task DeleteCascadeAsync(int solicitudId);

        /// <summary>
        /// Verifica si existe una solicitud con el número especificado.
        /// </summary>
        Task<bool> ExistsByRequestNumberAsync(string requestNumber);

        /// <summary>
        /// Remueve la política de pago asociada a las solicitudes que la tengan.
        /// </summary>
        Task RemovePaymentPolicyFromRequests(int paymentPolicyId);

        /// <summary>
        /// Actualiza la fecha máxima de completado de una solicitud.
        /// </summary>
        Task UpdateMaxDateAsync(int requestId, DateTime newDate);

        /// <summary>
        /// Obtiene una solicitud con todos sus detalles relacionados.
        /// </summary>
        Task<AcquisitionRequest?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Obtiene la lista de solicitudes vencidas.
        /// </summary>
        Task<List<ExpiredRequestDto>> GetExpiredRequestsAsync();

        /// <summary>
        /// Actualiza la metadata relacionada con notificaciones
        /// (ej. fecha de último envío).
        /// </summary>
        Task UpdateNotificationMetadataAsync(int requestId);

        /// <summary>
        /// Actualiza el estado de una solicitud.
        /// </summary>
        Task UpdateStatusAsync(int requestId, int newStatus);

        /// <summary>
        /// Obtiene los datos necesarios para evaluar el estado de una solicitud.
        /// </summary>
        Task<AcquisitionRequestStatusDto?> GetStatusDataAsync(int requestId);


        Task<List<AcquisitionRequest>> GetByClassificationId(int classificationId);
    }
}
