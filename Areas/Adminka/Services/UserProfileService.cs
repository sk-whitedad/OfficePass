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
                    Specialization = model.Specialization,
                    Group = model.Group,
                    User = model.User,
                };
                await userProfileRepository.Create(userProfile);
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
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<UserProfile>> GetUserProfiles()
        {
            try
            {
                var userProfiles = userProfileRepository.GetAll().ToList();
                if (userProfiles == null)
                {
                    return new BaseResponse<List<UserProfile>>
                    {
                        Description = "Профили не обнаружены",
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
                var userProfile = userProfileRepository.GetAll().FirstOrDefault(x => x.UserId == id);
                if (userProfile == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Профиль пользователя не найден"
                    };
                }

                userProfile.Firstname = model.Firstname;
                userProfile.Lastname = model.Lastname;
                userProfile.Surname = model.Surname;
                userProfile.Group = model.Group;
                userProfile.Specialization = model.Specialization;

                var result = await userProfileRepository.Update(userProfile);
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
                    Description = $"[UpdateUserProfile]: {ex.Message}"
                };
            }
        }
    }
}
