namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByNameAsync(string RolName);
        Task<Role?> GetByIdAsync(int idRol);
        Task AddAsync(Role role);
        Task<bool> UpdateAsync(Role role);
        Task<bool> DeleteAsync(int id);
    }
}
