using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using CommunityEntity = SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities.Community;



namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de comunidades.
    ///
    /// Permite realizar operaciones CRUD sobre las comunidades,
    /// incluyendo consultas por identificador y código, así como
    /// eliminación lógica.
    /// </summary>
    public interface ICommunityRepository
    {
        /// <summary>
        /// Agrega una nueva comunidad.
        /// </summary>
        Task<CommunityEntity?> AddAsync(CommunityEntity unit);

        /// <summary>
        /// Obtiene una comunidad mediante su código.
        /// </summary>
        Task<CommunityEntity?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todas las comunidades registradas.
        /// </summary>
        Task<IEnumerable<CommunityEntity>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de una comunidad existente.
        /// </summary>
        Task<bool> UpdateAsync(CommunityEntity unit);

        /// <summary>
        /// Realiza una eliminación lógica de la comunidad.
        /// </summary>
        Task<bool> DeleteAsync(int code, int idCommunity);

        /// <summary>
        /// Obtiene una comunidad por su identificador.
        /// </summary>
        Task<CommunityEntity?> GetByIdAsync(int idCommunity);
    }
}
