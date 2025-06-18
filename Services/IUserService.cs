using management_webapp_bn.DTOs;

namespace management_webapp_bn.Services
{
    public interface IUserService
    {
        Task<object> GetUsersDataTableAsync(DataTableRequestDto request);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<object> CreateUserAsync(UserCreateUpdateDto request);
        Task<object> UpdateUserAsync(string id, UserCreateUpdateDto request);
        Task<bool> DeleteUserAsync(string id);
    }
}
