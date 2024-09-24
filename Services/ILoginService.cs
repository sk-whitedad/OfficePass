using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Models;
using System.Security.Claims;

namespace OfficePass.Services
{
    public interface ILoginService
    {
        Task<IBaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<IBaseResponse<User>> GetUser(int id);

        Task<IBaseResponse<List<User>>> GetUsers();
    }
}
