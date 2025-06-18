using management_webapp_bn.Data;
using management_webapp_bn.DTOs;
using management_webapp_bn.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace management_webapp_bn.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<object> GetUsersDataTableAsync(DataTableRequestDto request)
        {
            var query = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                    .ThenInclude(up => up.Permission)
                .AsQueryable();

            // Search functionality
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(request.Search) ||
                    u.LastName.Contains(request.Search) ||
                    u.Email.Contains(request.Search) ||
                    u.Username.Contains(request.Search) ||
                    u.Phone.Contains(request.Search));
            }

            // Ordering
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var isDescending = request.OrderDirection?.ToLower() == "desc";

                query = request.OrderBy.ToLower() switch
                {
                    "firstname" => isDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                    "lastname" => isDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                    "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                    "username" => isDescending ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                    _ => isDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id)
                };
            }
            else
            {
                query = query.OrderBy(u => u.Id);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var pageNumber = int.TryParse(request.PageNumber, out var pn) ? pn : 1;
            var pageSize = int.TryParse(request.PageSize, out var ps) ? ps : 10;

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = new RoleDto
                    {
                        RoleId = u.Role.roleId,
                        RoleName = u.Role.roleName
                    },
                    Username = u.Username,
                    Password = u.Password,
                    Permission = u.Permissions.Select(up => new UserPermissionViewDto
                    {
                        PermissionId = up.PermissionId, // ใช้ existing PermissionId
                        PermissionName = up.Permission != null ? up.Permission.permissionName : "",
                        IsReadable = up.IsReadable,
                        IsWritable = up.IsWritable,
                        IsDeletable = up.IsDeletable
                    }).ToList()
                })
                .ToListAsync();

            return new
            {
                users = users,
                totalCount = totalCount,
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Role = new RoleDto
                {
                    RoleId = user.Role.roleId,
                    RoleName = user.Role.roleName
                },
                Username = user.Username,
                Password = user.Password,
                Permission = user.Permissions.Select(up => new UserPermissionViewDto
                {
                    PermissionId = up.PermissionId, // ใช้ existing PermissionId
                    PermissionName = up.Permission?.permissionName ?? "", // แสดง permissionName ถ้าไม่อยากให้แสดงก็ลบ
                    IsReadable = up.IsReadable,
                    IsWritable = up.IsWritable,
                    IsDeletable = up.IsDeletable
                }).ToList()
            };
        }

        public async Task<object> CreateUserAsync(UserCreateUpdateDto request)
        {
            // Check if user with same email or username already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email || u.Username == request.Username);

            if (existingUser != null)
            {
                throw new ArgumentException("User with this email or username already exists");
            }

            // Check if role exists
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.roleId == request.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }

            // Create new user
            var user = new Users
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request?.Phone,
                RoleId = request.RoleId,
                Username = request.Username,
                Password = request.Password // Note: In production, hash the password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Add user permissions using existing PermissionId
            if (request.Permission != null && request.Permission.Any())
            {
                var userPermissions = new List<UserPermission>();

                foreach (var permDto in request.Permission)
                {
                    // Check if permission exists
                    var permission = await _context.Permissions
                        .FirstOrDefaultAsync(p => p.permissionId == permDto.PermissionId);

                    if (permission != null)
                    {
                        userPermissions.Add(new UserPermission
                        {
                            Id = Guid.NewGuid().ToString(), // Auto-generate UserPermission ID
                            UserId = user.Id,
                            PermissionId = permDto.PermissionId, // Use existing Permission ID
                            IsReadable = permDto.IsReadable,
                            IsWritable = permDto.IsWritable,
                            IsDeletable = permDto.IsDeletable
                        });
                    }
                    else
                    {
                        // Log if permission not found
                        Console.WriteLine($"Permission {permDto.PermissionId} not found");
                    }
                }

                if (userPermissions.Any())
                {
                    _context.UserPermissions.AddRange(userPermissions);
                    await _context.SaveChangesAsync();
                }
            }

            // Get user with role for response
            var createdUser = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return new
            {
                firstName = createdUser.FirstName,
                lastName = createdUser.LastName,
                email = createdUser.Email,
                phone = createdUser.Phone,
                role = new
                {
                    roleId = createdUser.Role.roleId,
                    roleName = createdUser.Role.roleName
                },
                username = createdUser.Username,
                password = createdUser.Password,
                permission = createdUser.Permissions.Select(p => new
                {
                    permissionId = p.PermissionId ,
                    permissionName = p.Permission?.permissionName ?? ""

                }).ToList()
            };
        }

        public async Task<object> UpdateUserAsync(string id, UserCreateUpdateDto request)
        {
            var user = await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            // Check if email or username is being changed and already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id != id && (u.Email == request.Email || u.Username == request.Username));

            if (existingUser != null)
            {
                throw new ArgumentException("User with this email or username already exists");
            }

            // Check if role exists
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.roleId == request.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }

            // Update user properties
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.RoleId = request.RoleId;
            user.Username = request.Username;
            user.Password = request.Password; // Note: In production, hash the password

            // Remove existing permissions
            _context.UserPermissions.RemoveRange(user.Permissions);

            // Add updated permissions using existing PermissionId
            if (request.Permission != null && request.Permission.Any())
            {
                var userPermissions = new List<UserPermission>();

                foreach (var permDto in request.Permission)
                {
                    // Check if permission exists
                    var permission = await _context.Permissions
                        .FirstOrDefaultAsync(p => p.permissionId == permDto.PermissionId);

                    if (permission != null)
                    {
                        userPermissions.Add(new UserPermission
                        {
                            Id = Guid.NewGuid().ToString(), // Auto-generate UserPermission ID
                            UserId = user.Id,
                            PermissionId = permDto.PermissionId, // Use existing Permission ID
                            IsReadable = permDto.IsReadable,
                            IsWritable = permDto.IsWritable,
                            IsDeletable = permDto.IsDeletable
                        });
                    }
                    else
                    {
                        // Log if permission not found
                        Console.WriteLine($"Permission {permDto.PermissionId} not found");
                    }
                }

                if (userPermissions.Any())
                {
                    _context.UserPermissions.AddRange(userPermissions);
                }
            }

            await _context.SaveChangesAsync();

            // Get updated user with role for response
            var updatedUser = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return new
            {
                firstName = updatedUser.FirstName,
                lastName = updatedUser.LastName,
                email = updatedUser.Email,
                phone = updatedUser.Phone,
                role = new
                {
                    roleId = updatedUser.Role.roleId,
                    roleName = updatedUser.Role.roleName
                },
                username = updatedUser.Username,
                password = updatedUser.Password,
                permission = updatedUser.Permissions.Select(p => new
                {
                    permissionId = p.PermissionId, 
                    permissionName = p.Permission?.permissionName ?? "",
                    isReadable = p.IsReadable,
                    isWritable = p.IsWritable,
                    isDeletable = p.IsDeletable
                }).ToList()
            };
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return false;

            // Remove user permissions first
            _context.UserPermissions.RemoveRange(user.Permissions);

            // Remove user
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
