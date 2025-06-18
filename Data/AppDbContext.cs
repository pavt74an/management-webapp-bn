using management_webapp_bn.Models;
using Microsoft.EntityFrameworkCore;

namespace management_webapp_bn.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //def. primary key is Id
            modelBuilder.Entity<Users>().HasKey(u => u.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.roleId);
            modelBuilder.Entity<Permission>().HasKey(p => p.permissionId);
            modelBuilder.Entity<UserPermission>().HasKey(up => up.Id);

            //def. relation between UsersPermission and Users
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(up => up.UserId);

            //def. relation between UsersPermission and Permission
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);
                
        }
    }
}
