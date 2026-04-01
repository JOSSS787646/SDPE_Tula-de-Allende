using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Interfaz que expone la información del usuario autenticado en el contexto actual.
///
/// Proporciona acceso a datos básicos como Id, correo y rol del usuario,
/// permitiendo utilizarlos en la lógica de negocio sin depender directamente
/// del contexto de la infraestructura (ej. HttpContext).
/// </summary>
/// 



namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        string Email { get; }

        string Role { get; }
    }
}
