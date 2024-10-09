using OfficePass.Areas.Adminka.Models;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Areas.Adminka.Services
{
    public interface IUserService
    {
        Task<IBaseResponse<List<User>>> GetUsers();

        Task <IBaseResponse<User>> CreateUser(User model);

        IBaseResponse<User> GetUserById(int id);

        Task <IBaseResponse<bool>> DeleteUser(int id);

        Task<IBaseResponse<bool>> UpdateUser(User model);

        Task<IBaseResponse<bool>> DublicateUser(string login);

        Task<IBaseResponse<bool>> DublicateUserProfile(int id);

    }
}
