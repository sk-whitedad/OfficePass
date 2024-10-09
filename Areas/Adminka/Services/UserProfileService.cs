using Microsoft.EntityFrameworkCore;
using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Areas.Adminka.Services
{
    public class UserProfileService: IUserProfileService
    {
        private readonly UserProfileRepository userProfileRepository;

        public UserProfileService(UserProfileRepository userProfileRepository)
        {
            if (userProfileRepository == null) throw new ArgumentNullException(nameof(userProfileRepository));
            this.userProfileRepository = userProfileRepository;
        }

        public async Task<IBaseResponse<UserProfile>> CreateUserProfile(UserProfile model)
        {
            try
            {
                var userProfile = new UserProfile()
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    GroupId = model.GroupId,
                    SpecializationId = model.SpecializationId,
                    IsBoss = model.IsBoss,
                };
                await userProfileRepository.Create(userProfile);
                return new BaseResponse<UserProfile>()
                {
                    StatusCode = StatusCode.OK,
                    Data = userProfile,
                    Description = $"Сотрудник успешно добавлен"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<UserProfile>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public  IBaseResponse<List<UserProfile>> GetUserProfiles()
        {
            try
            {
                var userProfiles = userProfileRepository.GetAll()
                    .Include(x => x.Specialization)
                    .Include(x => x.Group)
                    .Include(x => x.User)
                    .ToList();
                if (userProfiles == null)
                {
                    return new BaseResponse<List<UserProfile>>
                    {
                        Description = "Сотрудники не обнаружены",
                        StatusCode = Enums.StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<UserProfile>>()
                {
                    Data = userProfiles,
                    StatusCode = Enums.StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserProfile>>
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = Enums.StatusCode.InternalServerError
                };             
            }
        }

        public async Task<IBaseResponse<bool>> UpdateUserProfile(int id, UserProfile model)
        {
            try
            {
                var userProfile = userProfileRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (userProfile == null)
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Профиль сотрудника не найден"
                    };
                userProfile.Firstname = model.Firstname;
                userProfile.Lastname = model.Lastname;
                userProfile.Surname = model.Surname;
                userProfile.PhoneNumber = model.PhoneNumber;
                userProfile.GroupId = model.GroupId;
                userProfile.SpecializationId = model.SpecializationId;
                userProfile.IsBoss = model.IsBoss;
                var result = await userProfileRepository.Update(userProfile);
                if (result)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                        Description = "Профиль сотрудника успешно изменен"
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
                    Description = $"[UpdateUserProfile]: {ex.Message}"
                };
            }
        }

        public IBaseResponse<UserProfile> GetUserProfileById(int id)
        {
            try
            {
                var userProfile = userProfileRepository.GetAll()
                    .Include(x => x.Group)
                    .Include(x => x.Specialization)
                    .FirstOrDefault(x => x.Id == id);
                if (userProfile == null)
                {
                    return new BaseResponse<UserProfile>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Сотрудник не найден"
                    };
                }
                return new BaseResponse<UserProfile>()
                {
                    StatusCode = StatusCode.OK,
                    Data = userProfile
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserProfile>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[GetUserById]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUserProfile(int id)
        {
            try
            {
                var user = userProfileRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Сотрудник не найден"
                    };
                }

                await userProfileRepository.Delete(user);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = $"Сотрудник успешно удален"
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
