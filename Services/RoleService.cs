using management_webapp_bn.Data;
using management_webapp_bn.Models;
using Microsoft.EntityFrameworkCore;

namespace management_webapp_bn.Services
{
    public class RoleService :IRoleService
    {
        private readonly AppDbContext _context;
        public RoleService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Role> CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
