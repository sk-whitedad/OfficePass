
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
        private readonly UserRepository userRepository;

        public UserService(UserRepository userRepository, RoleRepository roleRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            this.userRepository = userRepository;
        }

        public async Task<IBaseResponse<List<User>>> GetUsers()
        {
            try
            {
                var users = userRepository.GetAll()
                    .Include(r => r.Role)
                    .Include(r => r.UserProfile)
                    .ToList();
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
                    UserProfileId = model.UserProfileId,
                    RoleId = model.RoleId
                };
                await userRepository.Create(user);

                return new BaseResponse<User>
                {
                    StatusCode = StatusCode.OK,
                    Data = user,
                    Description = $"Новый пользователь {user.Login} успешно создан"
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
                var user = userRepository.GetAll()
                    .Include(x => x.Role)
                    .Include(x => x.UserProfile)
                        .ThenInclude(y => y!.Specialization)
                    .Include(x => x.UserProfile)
                        .ThenInclude(z => z!.Group)
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
                var user = userRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if(user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }

                await userRepository.Delete(user);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Пользователь успешно удален"
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
                var user = userRepository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }

                user.RoleId = model.RoleId;
                if (!String.IsNullOrEmpty(model.Password))
                    user.Password = model.Password;

                var result = await userRepository.Update(user);
                if (result)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                        Description = "Изменения сохранены"
                    };
                else
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UpdateDBError,
                        Description = "Ошибка базы данных при запросе"                        
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

        public async Task<IBaseResponse<bool>> DublicateUser(string login)
        {
            try
            {
                var user = userRepository.GetAll().FirstOrDefault(x => x.Login == login);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                    };
                }

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.IsExists,
                    Description = "Такой логин уже существует"
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
    
        public async Task<IBaseResponse<bool>> DublicateUserProfile(int id)
        {
            try
            {
                var user = userRepository.GetAll().FirstOrDefault(x => x.UserProfileId == id);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                        Description = "Дубликатов профилей не найдено"
                    };
                }

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.IsExists,
                    Description = "Выбранный сотрудник уже имеет логин"
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
    }
}
