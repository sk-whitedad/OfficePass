using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using OfficePass.Areas.Adminka.Domain.Repositories;
using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Services
{
    public class GuestService : IGuestService
    {
        private readonly GuestRepository guestRepository;

        public GuestService(GuestRepository guestRepository)
        {
            if (guestRepository == null) throw new ArgumentNullException(nameof(guestRepository));
            this.guestRepository = guestRepository;
        }


        public async Task<IBaseResponse<Guest>> CreateGuest(Guest model)
        {
            try
            {
                var guest = new Guest
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SurName = model.SurName,
                    DocumentSerial = model.DocumentSerial,
                    DocumentNumber = model.DocumentNumber,
                    DocumentTypeId = model.DocumentTypeId,
                    CompanyId = model.CompanyId,
                    UserId = model.UserId
                };
                await guestRepository.Create(guest);
                return new BaseResponse<Guest>
                {
                    StatusCode = StatusCode.OK,
                    Data = guest,
                    Description = $"Посетитель успешно добавлен"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guest>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[CreateGuest]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteGuest(int id)
        {
            try
            {
                var guest = guestRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (guest == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Посетитель не найден"
                    };
                }

                await guestRepository.Delete(guest);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = $"Посетитель успешно удален"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[DeleteGuest]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DublicateGuest(string str1, string str2)
        {
            try
            {
                var guest = guestRepository.GetAll().FirstOrDefault(x => x.DocumentSerial == str1 && x.DocumentNumber == str2);
                if (guest == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                    };
                }

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.IsExists,
                    Description = "Такой посетитель уже существует"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[DeleteGuest]: {ex.Message}"
                };
            }
        }

        public IBaseResponse<Guest> GetGuestById(int id)
        {
            try
            {
                var guest = guestRepository.GetAll().FirstOrDefault(x => x.Id == id);
                if (guest == null)
                {
                    return new BaseResponse<Guest>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Посетитель не найден"
                    };
                }
                return new BaseResponse<Guest>()
                {
                    StatusCode = StatusCode.OK,
                    Data = guest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guest>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"[GetGuestById]: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Guest>>> GetGuests()
        {
            try
            {
                var guest = guestRepository.GetAll()
                    .Include(x => x.Company)
                    .Include(x => x.DocumentType)
                    .ToList();
                if (guest == null)
                {
                    return new BaseResponse<List<Guest>>
                    {
                        Description = "Посетители не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<Guest>>
                {
                    Data = guest,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Guest>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> UpdateGuest(Guest model)
        {
            try
            {
                var guest = guestRepository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (guest == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NotFound,
                        Description = "Посетитель не найден"
                    };
                }
                guest.FirstName = model.FirstName;
                guest.LastName = model.LastName;
                guest.SurName = model.SurName;
                guest.DocumentSerial = model.DocumentSerial;
                guest.DocumentNumber = model.DocumentNumber;
                guest.DocumentTypeId = model.DocumentTypeId;
                guest.CompanyId = model.CompanyId;

                var result = await guestRepository.Update(guest);
                if (result)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK,
                        Description = "Посетитель успешно изменен"
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
                    Description = $"[UpdateGuest]: {ex.Message}"
                };
            }
        }
    }
}
