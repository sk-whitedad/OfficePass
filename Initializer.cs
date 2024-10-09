using NuGet.ContentModel;
using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Areas.Adminka.Services;
using OfficePass.Domain.Repositories;
using OfficePass.Models;
using OfficePass.Services;

namespace OfficePass
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<LoginRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<UserProfileRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<SpecializationRepository>();

        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
        }
    }
}
