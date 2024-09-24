using Microsoft.EntityFrameworkCore;
using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Areas.Adminka.Models;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;
using System.Security.Claims;

namespace OfficePass.Areas.Adminka.Services
{
    public class UserService : IUserService
    {
        private readonly UsersSettingsRepository usersSettingsRepository;

        public UserService(UsersSettingsRepository usersSettingsRepository, RoleRepository roleRepository)
        {
            if (usersSettingsRepository == null) throw new ArgumentNullException(nameof(usersSettingsRepository));
            this.usersSettingsRepository = usersSettingsRepository;
        }

        public async Task<IBaseResponse<List<User>>> GetUsers()
        {
            try
            {
                var users = usersSettingsRepository.GetAll().Include(r => r.Role).ToList();
                if (users == null)
                {
                    return new BaseResponse<List<User>>
                    {
                        Description = "Пользователи не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<User>>
                {
                    Data = users,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<User>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> CreateUser(User model)
        {
            try
            {
                var user = new User()
                {
                    Login = model.Login,
                    Password = model.Password,
                    Role = model.Role,
                    UserProfile = model.UserProfile
                };
                await usersSettingsRepository.Create(model);

                return new BaseResponse<User>
                {
                    StatusCode = StatusCode.OK,
                    Data = user,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };                
            }
        }

        public IBaseResponse<User> GetUserById(int id)
        {
            try
            {
                var user = usersSettingsRepository.GetAll()
                    .Include(x => x.Role)
                    .Include(x => x.UserProfile)
                    .FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<User>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }
                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[GetUserById]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUser(int id)
        {
            try
            {
                var user = usersSettingsRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if(user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }

                await usersSettingsRepository.Delete(user);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[DeleteUser]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> UpdateUser(User model)
        {
            try
            {
                var user = usersSettingsRepository.GetAll()
                    .Include(x => x.Role)
                    .Include(x => x.UserProfile)
                    .FirstOrDefault(x => x.Id == model.Id);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }
                user.Login = model.Login;
                if (!String.IsNullOrEmpty(model.Password))
                    user.Password = model.Password;

                var result = await usersSettingsRepository.Update(user);
                if (result)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK
                    };
                else
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UpdateDBError
                    };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[UpdateUser]: {ex.Message}"
                };
            }
        }
    }
}
