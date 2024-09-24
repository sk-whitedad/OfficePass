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
            services.AddScoped<UsersSettingsRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<UserProfileRepository>();
            services.AddScoped<CompanyRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ICompanyService, CompanyService>();
        }
    }
}
