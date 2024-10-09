using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Services
{
    public interface IGroupService
    {
        Task<IBaseResponse<List<Group>>> GetGroups();

        Task<IBaseResponse<Group>> CreateGroup(Group model);

        IBaseResponse<Group> GetGroupById(int id);

        Task<IBaseResponse<bool>> DeleteGroup(int id);

        Task<IBaseResponse<bool>> UpdateGroup(Group model);
    }
}