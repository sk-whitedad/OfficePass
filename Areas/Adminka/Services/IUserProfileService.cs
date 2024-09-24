using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Areas.Adminka.Services
{
    public interface IUserProfileService
    {
        IBaseResponse<List<UserProfile>> GetUserProfiles();

        Task<IBaseResponse<UserProfile>> CreateUserProfile(UserProfile model);

        Task<IBaseResponse<bool>> UpdateUserProfile(int id, UserProfile model);
    }
}
