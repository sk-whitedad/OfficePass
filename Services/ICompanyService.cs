using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Services
{
    public interface ICompanyService
    {
        Task<IBaseResponse<List<Company>>> GetCompanies();

        Task<IBaseResponse<Company>> CreateCompany(Company model);

        IBaseResponse<Company> GetCompanyById(int id);

        Task<IBaseResponse<bool>> DeleteCompany(int id);

        Task<IBaseResponse<bool>> UpdateCompany(Company model);
    }
}
