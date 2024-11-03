using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Areas.Adminka.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly SpecializationRepository specializationRepository;

        public SpecializationService(SpecializationRepository specializationRepository)
        {
            if (specializationRepository == null) throw new ArgumentNullException(nameof(specializationRepository));
            this.specializationRepository = specializationRepository;
        }

        public async Task<IBaseResponse<Specialization>> CreateSpecialization(Specialization model)
        {
            try
            {
                var specialization = new Specialization
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                await specializationRepository.Create(specialization);
                return new BaseResponse<Specialization>
                {
                    StatusCode = StatusCode.OK,
                    Data = specialization,
                    Description = "Новая должность успешно добавлена"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Specialization>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[CreateSpecialization]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteSpecialization(int id)
        {
            try
            {
                var specialization = specializationRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (specialization == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Должность не найдена"
                    };
                }

                await specializationRepository.Delete(specialization);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Должность успешно удалена"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[DeleteSpecialization]: {ex.Message}"
                };
            }
        }

        public IBaseResponse<List<Specialization>> GetSpecializations()
        {
            try
            {
                var specializations = specializationRepository.GetAll().ToList();
                if (specializations == null)
                {
                    return new BaseResponse<List<Specialization>>
                    {
                        Description = "Должности не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<Specialization>>
                {
                    Data = specializations,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Specialization>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }

        }

        public IBaseResponse<Specialization> GetSpecializationById(int id)
        {
            try
            {
                var specialization = specializationRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (specialization == null)
                {
                    return new BaseResponse<Specialization>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Должность не найдена"
                    };
                }
                return new BaseResponse<Specialization>()
                {
                    StatusCode = StatusCode.OK,
                    Data = specialization
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Specialization>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[GetSpecializationById]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> UpdateSpecialization(Specialization model)
        {
            try
            {
                var specialization = specializationRepository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (specialization == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Должность не найдена"
                    };
                }
                specialization.Name = model.Name;
                specialization.Description = model.Description;

                var result = await specializationRepository.Update(specialization);
                if (result)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                        Description = "Должность успешно изменена"
                    };
                else
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.UpdateDBError,
                        Description = "Не удалось изменить должность"
                    };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[UpdateSpecialization]: {ex.Message}"
                };
            }

        }
    }
}
