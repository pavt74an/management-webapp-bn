using management_webapp_bn.Models;

namespace management_webapp_bn.Services
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(Role role);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
