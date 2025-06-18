using management_webapp_bn.DTOs;
using management_webapp_bn.Models;
using management_webapp_bn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace management_webapp_bn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto dto)
        {
            if (string.IsNullOrEmpty(dto.PermissionId) || string.IsNullOrEmpty(dto.PermissionName))
                return BadRequest("PermissionId and PermissionName are required.");

            var permission = new Permission
            {
                permissionId = dto.PermissionId,
                permissionName = dto.PermissionName,
                UserPermissions = new List<UserPermission>() // ป้องกัน null error
            };

            var createdPermission = await _permissionService.CreatePermissionAsync(permission);

            return CreatedAtAction(nameof(GetAllPermissions), new { id = createdPermission.permissionId }, createdPermission);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();

            var data = permissions.Select(p => new
            {
                permissionId = p.permissionId,
                permissionName = p.permissionName
            }).ToList();

            return Ok(new
            {
                status = new
                {
                    code = 200,
                    description = "Permissions retrieved successfully."
                },
                data
            });
        }


    }
}
