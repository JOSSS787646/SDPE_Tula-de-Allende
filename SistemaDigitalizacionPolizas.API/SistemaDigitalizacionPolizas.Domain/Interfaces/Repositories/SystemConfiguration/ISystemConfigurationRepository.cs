using SistemaDigitalizacionPolizas.Domain.Entities.SystemConfiguration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de la configuración del sistema.
    ///
    /// Permite obtener, crear y actualizar la configuración activa utilizada
    /// para el funcionamiento general de la aplicación.
    /// </summary>
    public interface ISystemConfigurationRepository
    {
        /// <summary>
        /// Obtiene la configuración activa del sistema.
        /// </summary>
        Task<SystemConfiguration?> GetActiveAsync();

        /// <summary>
        /// Registra una nueva configuración del sistema.
        /// </summary>
        Task AddAsync(SystemConfiguration configuration);

        /// <summary>
        /// Actualiza la configuración del sistema existente.
        /// </summary>
        Task UpdateAsync(SystemConfiguration configuration);
    }
}
