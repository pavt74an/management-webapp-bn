namespace management_webapp_bn.DTOs
{
    public class UserPermissionDto
    {
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }
    }
}
