namespace management_webapp_bn.Models
{
    public class Role
    {
        public string roleId { get; set; }
        public string roleName { get; set; }
        public ICollection<Users> Users { get; set; }

    }
}
