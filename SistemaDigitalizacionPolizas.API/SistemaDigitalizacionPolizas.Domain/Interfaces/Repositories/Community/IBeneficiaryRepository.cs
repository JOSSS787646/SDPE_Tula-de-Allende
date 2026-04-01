using SistemaDigitalizacionPolizas.Domain.Dtos.Beneficiary;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de beneficiarios.
    ///
    /// Permite realizar operaciones de registro, consulta, validación,
    /// actualización y eliminación de beneficiarios, así como verificar
    /// la existencia de datos únicos como CURP e INE.
    /// </summary>
    public interface IBeneficiaryRepository
    {
        /// <summary>
        /// Agrega un nuevo beneficiario.
        /// </summary>
        Task<Beneficiary> AddAsync(Beneficiary unit);

        /// <summary>
        /// Verifica si existe un beneficiario con la CURP especificada.
        /// </summary>
        Task<bool> ExistsByCurpAsync(string curp);

        /// <summary>
        /// Verifica si existe un beneficiario con la INE especificada.
        /// </summary>
        Task<bool> ExistsByIneAsync(string ine);

        /// <summary>
        /// Obtiene todos los beneficiarios en formato DTO.
        /// </summary>
        Task<List<BeneficiaryDto>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un beneficiario existente.
        /// </summary>
        Task<bool> UpdateAsync(Beneficiary unit);

        /// <summary>
        /// Realiza una eliminación lógica del beneficiario,
        /// validando por CURP y su identificador.
        /// </summary>
        Task<bool> DeleteAsync(string curp, int beneficiaryId);

        /// <summary>
        /// Obtiene un beneficiario por su identificador.
        /// </summary>
        Task<Beneficiary?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene un beneficiario por su CURP.
        /// </summary>
        Task<Beneficiary?> GetByCurpAsync(string curp);
    }
}
