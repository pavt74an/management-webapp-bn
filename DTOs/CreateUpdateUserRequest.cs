namespace management_webapp_bn.DTOs
{
    public class CreateUpdateUserRequest
    {
        public string? Id { get; set; } // Only for create
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<PermissionRequest> Permission { get; set; } = new List<PermissionRequest>();
    }
    public class PermissionRequest
    {
        public string PermissionId { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }
    }
}
