using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de responsables
    /// o administradores de solicitudes.
    ///
    /// Permite consultar, crear y actualizar la información de los encargados
    /// asociados a las solicitudes.
    /// </summary>
    public interface IRequestManagerRepository
    {
        /// <summary>
        /// Obtiene la lista de responsables de solicitudes en formato de vista previa.
        /// </summary>
        Task<IEnumerable<RequestManagerPreviewDto>> GetAllAsync();

        /// <summary>
        /// Crea un nuevo responsable de solicitud y retorna su identificador.
        /// </summary>
        Task<int> AddAsync(CreateRequestManagerDto dto);

        /// <summary>
        /// Actualiza la información de un responsable de solicitud.
        /// </summary>
        Task<bool> UpdateAsync(UpdateRequestManagerDto dto);
    }
}
