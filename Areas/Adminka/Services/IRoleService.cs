using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Areas.Adminka.Services
{
    public interface IRoleService
    {
        IBaseResponse<List<Role>> GetRoles();
    }
}
