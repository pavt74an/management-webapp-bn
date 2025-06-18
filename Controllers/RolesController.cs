using management_webapp_bn.DTOs;
using management_webapp_bn.Models;
using management_webapp_bn.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management_webapp_bn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            if (string.IsNullOrEmpty(roleDto.RoleId) || string.IsNullOrEmpty(roleDto.RoleName))
                return BadRequest("RoleId and RoleName are required.");

            var role = new Role
            {
                roleId = roleDto.RoleId,
                roleName = roleDto.RoleName,
                Users = new List<Users>() 
            };

            var createdRole = await _roleService.CreateRoleAsync(role);

            return CreatedAtAction(nameof(GetAllRoles), new { id = createdRole.roleId }, createdRole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();

                var data = roles.Select(r => new
                {
                    roleId = r.roleId,
                    roleName = r.roleName
                }).ToList();

                return Ok(new
                {
                    status = new
                    {
                        code = 200,
                        description = "Roles retrieved successfully."
                    },
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = new
                    {
                        code = 500,
                        description = "An error occurred while retrieving roles."
                    },
                    error = ex.Message
                });
            }
        }

    }
}
