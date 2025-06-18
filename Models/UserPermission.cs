namespace management_webapp_bn.Models
{
    public class UserPermission
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PermissionId { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }
        public Users User { get; set; }
        public Permission Permission { get; set; }

    }
}
