namespace management_webapp_bn.Models
{
    public class Permission
    {
        public string permissionId { get; set; }
        public string permissionName { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
