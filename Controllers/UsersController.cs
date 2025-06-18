using management_webapp_bn.DTOs;
using management_webapp_bn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace management_webapp_bn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // Get Users with DataTable
        [HttpPost("DataTable")]
        public async Task<IActionResult> GetUsersDataTable([FromBody] DataTableRequestDto request)
        {
            try
            {
                var result = await _userService.GetUsersDataTableAsync(request);
                return Ok(new
                {
                    status = new { code = "200", description = "Success" },
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = new { code = "500", description = "Internal Server Error" },
                    data = new { message = ex.Message }
                });
            }
        }

        // Get User By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new
                    {
                        status = new { code = "404", description = "User not found" },
                        data = (object)null
                    });
                }

                return Ok(new
                {
                    status = new { code = "200", description = "Success" },
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = new { code = "500", description = "Internal Server Error" },
                    data = new { message = ex.Message }
                });
            }
        }

        // Add New User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateUpdateDto request)
        {
            try
            {
                var result = await _userService.CreateUserAsync(request);
                return Ok(new
                {
                    status = new { code = "201", description = "User created successfully" },
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    status = new { code = "400", description = "Bad Request" },
                    data = new { message = ex.Message }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = new { code = "500", description = "Internal Server Error" },
                    data = new { message = ex.Message }
                });
            }
        }

        // Edit User
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserCreateUpdateDto request)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, request);
                if (result == null)
                {
                    return NotFound(new
                    {
                        status = new { code = "404", description = "User not found" },
                        data = (object)null
                    });
                }

                return Ok(new
                {
                    status = new { code = "200", description = "User updated successfully" },
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    status = new { code = "400", description = "Bad Request" },
                    data = new { message = ex.Message }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = new { code = "500", description = "Internal Server Error" },
                    data = new { message = ex.Message }
                });
            }
        }

        // Delete User
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound(new
                    {
                        status = new { code = "404", description = "User not found" },
                        data = new { result = false, message = "User not found" }
                    });
                }

                return Ok(new
                {
                    status = new { code = "200", description = "User deleted successfully" },
                    data = new { result = true, message = "User deleted successfully" }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = new { code = "500", description = "Internal Server Error" },
                    data = new { result = false, message = ex.Message }
                });
            }
        }

    }
}
