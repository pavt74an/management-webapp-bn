namespace management_webapp_bn.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string RoleId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public List<UserPermissionDto> Permissions { get; set; } = new List<UserPermissionDto>();
    }
}
