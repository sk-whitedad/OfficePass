using Microsoft.EntityFrameworkCore;
using OfficePass.Domain.Entities;
using OfficePass.Helpers;

namespace OfficePass.Domain
{
    public partial class OfficepassdbContext : DbContext
    {
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Company> Companies { get; set; }

        public OfficepassdbContext(DbContextOptions<OfficepassdbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<UserProfile>().HasData(new UserProfile
            {
                Id = 1,
                UserId = 1,
            });

            builder.Entity<UserProfile>().HasData(new UserProfile
            {
                Id = 2,
                UserId = 2,
            });

            builder.Entity<Role>().HasData(new Role
                {
                    Id = 1,
                    Name = "Admin"
                });

            builder.Entity<Role>().HasData(new Role
            {
                Id = 2,
                Name = "User"
            });

            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                Login = "Admin",
                Password = HashPasswordHelper.HashPassowrd("123"),
                RoleId = 1,
           });

            builder.Entity<User>().HasData(new User
            {
                Id = 2,
                Login = "User",
                Password = HashPasswordHelper.HashPassowrd("123"),
                RoleId = 2,
            });
        }
    }
}

//dotnet ef migrations add Initial_BD
//dotnet ef database update