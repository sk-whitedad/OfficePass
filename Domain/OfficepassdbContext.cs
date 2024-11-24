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
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }

        public OfficepassdbContext(DbContextOptions<OfficepassdbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Group>().HasData(new Group
            {
                Id = 1,
                Name = "IT"
            });

            builder.Entity<Specialization>().HasData(new Specialization
            {
                Id = 1,
                Name = "Системный администратор"
            });

            builder.Entity<UserProfile>().HasData(new UserProfile
            {
                Id = 1,
                Firstname = "Иван",
                Lastname = "Иванов",
                Surname = "Иванович",
                PhoneNumber = "000",
                SpecializationId = 1,
                GroupId = 1,
                IsBoss = true,
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
                UserProfileId = 1,
           });

            builder.Entity<DocumentType>().HasData(new DocumentType
            {
                Id = 1,
                Name = "Паспорт РФ"
            });

            builder.Entity<DocumentType>().HasData(new DocumentType
            {
                Id = 2,
                Name = "Водительское удостоверение"
            });

            builder.Entity<DocumentType>().HasData(new DocumentType
            {
                Id = 3,
                Name = "Загранпаспорт"
            });

            builder.Entity<DocumentType>().HasData(new DocumentType
            {
                Id = 4,
                Name = "Паспорт моряка"
            });

        }
    }
}

//dotnet ef migrations add Initial_BD
//dotnet ef database update