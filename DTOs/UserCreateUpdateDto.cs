namespace management_webapp_bn.DTOs
{
    public class UserCreateUpdateDto
    {
        public string? Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<UserPermissionDto> Permission { get; set; }
    }
}
