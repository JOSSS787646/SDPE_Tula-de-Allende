using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de proveedores.
    ///
    /// Permite realizar operaciones de registro, consulta, validación,
    /// actualización y eliminación de proveedores, incluyendo verificaciones
    /// de unicidad como RFC y teléfono.
    /// </summary>
    public interface ISupplierRepository
    {
        /// <summary>
        /// Agrega un nuevo proveedor.
        /// </summary>
        Task<Supplier> AddAsync(Supplier unit);

        /// <summary>
        /// Verifica si existe un proveedor con el RFC especificado.
        /// </summary>
        Task<bool> ExistsByRfcAsync(string rfc);

        /// <summary>
        /// Obtiene todos los proveedores registrados.
        /// </summary>
        Task<IEnumerable<Supplier>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un proveedor existente.
        /// </summary>
        Task<bool> UpdateAsync(Supplier unit);

        /// <summary>
        /// Realiza una eliminación lógica del proveedor,
        /// validando por RFC y su identificador.
        /// </summary>
        Task<bool> DeleteAsync(string rfc, int supplierid);

        /// <summary>
        /// Obtiene un proveedor por su identificador.
        /// </summary>
        Task<Supplier?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene un proveedor por su RFC.
        /// </summary>
        Task<Supplier?> GetByRfcAsync(string rfc);

        /// <summary>
        /// Verifica si existe un proveedor con el teléfono especificado.
        /// </summary>
        Task<bool> ExistsByPhoneAsync(string phone);

        /// <summary>
        /// Obtiene proveedores de forma paginada.
        /// </summary>
        Task<List<Supplier>> GetPagedAsync(int page, int pageSize);
    }
}
