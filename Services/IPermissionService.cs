using management_webapp_bn.Models;

namespace management_webapp_bn.Services
{
    public interface IPermissionService
    {
        Task<Permission> CreatePermissionAsync(Permission permission);
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    }
}
