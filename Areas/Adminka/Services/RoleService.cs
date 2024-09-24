using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Areas.Adminka.Services
{
    public class RoleService: IRoleService
    {
        private readonly RoleRepository roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            if (roleRepository == null) throw new ArgumentNullException(nameof(roleRepository));
            this.roleRepository = roleRepository;
        }

        public IBaseResponse<List<Role>> GetRoles()
        {
            try
            {
                var roles = roleRepository.GetAll().ToList();
                if (roles == null)
                {
                    return new BaseResponse<List<Role>>
                    {
                        Description = "Роли не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<Role>>
                {
                    Data = roles,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Role>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
