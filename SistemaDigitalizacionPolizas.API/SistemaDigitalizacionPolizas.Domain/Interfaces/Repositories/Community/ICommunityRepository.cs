using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using CommunityEntity = SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities.Community;



namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community
{
    public interface ICommunityRepository
    {

        //SE DECLARO UNA VARIBALE PARA GUADAR EL VALOR DE LA INTERFAZ DEBIDO A 
        //INCOHERENCIA DE NOMBRES ENTRE LA ENTIDAD Y LA INTERFAZ, SE DECLARO COMO COMMUNITYENTITY PARA EVITAR CONFLICTOS
        Task<CommunityEntity?> AddAsync(CommunityEntity unit);
        Task<CommunityEntity?> GetByCodeAsync(int code);
        Task<IEnumerable<CommunityEntity>> GetAllAsync();
        Task<bool> UpdateAsync(CommunityEntity unit);
        Task<bool> DeleteAsync(int code, int idCommunity);
        Task<CommunityEntity?> GetByIdAsync(int idCommunity);

    }
}
