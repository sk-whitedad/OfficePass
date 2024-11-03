using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Areas.Adminka.Services
{
    public interface ISpecializationService
    {
        IBaseResponse<List<Specialization>> GetSpecializations();

        Task<IBaseResponse<Specialization>> CreateSpecialization(Specialization model);

        IBaseResponse<Specialization> GetSpecializationById(int id);

        Task<IBaseResponse<bool>> DeleteSpecialization(int id);

        Task<IBaseResponse<bool>> UpdateSpecialization(Specialization model);
    }
}
