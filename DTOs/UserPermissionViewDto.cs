namespace management_webapp_bn.DTOs
{
    public class UserPermissionViewDto
    {
        public string PermissionId { get; set; } //auto-generate
        public string PermissionName { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }
    }
}
