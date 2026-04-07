using SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de políticas de pago.
    ///
    /// Permite crear, consultar, actualizar y eliminar políticas de pago,
    /// así como obtener listados paginados y políticas disponibles.
    /// </summary>
    public interface IPaymentPolicyRepository
    {
        /// <summary>
        /// Crea una nueva política de pago.
        /// </summary>
        Task<PaymentPolicy> CreateAsync(PaymentPolicy policy);

        /// <summary>
        /// Obtiene una política de pago por su identificador.
        /// </summary>
        Task<PaymentPolicy?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una política de pago mediante su código.
        /// </summary>
        Task<PaymentPolicy?> GetByPolicyCodeAsync(string policyCode);

        /// <summary>
        /// Obtiene políticas de pago de forma paginada.
        /// </summary>
        Task<List<PaymentPolicy>> GetPagedAsync(int page, int pageSize);

        /// <summary>
        /// Actualiza la información de una política de pago existente.
        /// </summary>
        Task UpdateAsync(PaymentPolicy policy);

        /// <summary>
        /// Obtiene las políticas de pago disponibles para uso.
        /// </summary>
        Task<List<PaymentPolicy>> GetAvailablePoliciesAsync();

        /// <summary>
        /// Elimina una política de pago por su identificador.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
