using management_webapp_bn.Data;
using management_webapp_bn.Models;
using Microsoft.EntityFrameworkCore;

namespace management_webapp_bn.Services
{
    public class PermissionService :IPermissionService
    {
        private readonly AppDbContext _context;
        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }
    }
}
