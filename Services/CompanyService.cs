using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly CompanyRepository companyRepository;

        public CompanyService(CompanyRepository companyRepository)
        {
            if (companyRepository == null) throw new ArgumentNullException(nameof(companyRepository));
            this.companyRepository = companyRepository;
        }

        public async Task<IBaseResponse<Company>> CreateCompany(Company model)
        {
            try
            {
                var company = new Company
                {
                    Name = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                };
                await companyRepository.Create(company);
                return new BaseResponse<Company>
                {
                    StatusCode = StatusCode.OK,
                    Data = company
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Company>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[CreateCompany]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteCompany(int id)
        {
            try
            {
                var company = companyRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (company == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Компания не найдена"
                    };
                }

                await companyRepository.Delete(company);
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
                    Description = $"[DeleteCompany]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Company>>> GetCompanies()
        {
            try
            {
                var company = companyRepository.GetAll().ToList();
                if (company == null)
                {
                    return new BaseResponse<List<Company>>
                    {
                        Description = "Компании не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<Company>>
                {
                    Data = company,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Company>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<Company> GetCompanyById(int id)
        {
            try
            {
                var company = companyRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (company == null)
                {
                    return new BaseResponse<Company>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Компания не найдена"
                    };
                }
                return new BaseResponse<Company>()
                {
                    StatusCode = StatusCode.OK,
                    Data = company
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Company>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[GetCompanyById]: {ex.Message}"
                };
            }

        }

        public async Task<IBaseResponse<bool>> UpdateCompany(Company model)
        {
            try
            {
                var company = companyRepository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (company == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Компания не найдена"
                    };
                }
                company.Name = model.Name;
                company.Address = model.Address;
                company.PhoneNumber = model.PhoneNumber;

                var result = await companyRepository.Update(company);
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
                    Description = $"[UpdateCompany]: {ex.Message}"
                };
            }

        }
    }
}
