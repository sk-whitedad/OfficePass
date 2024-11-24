using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Services
{
    public interface IGuestService
    {
        Task<IBaseResponse<List<Guest>>> GetGuests();

        Task<IBaseResponse<Guest>> CreateGuest(Guest model);

        IBaseResponse<Guest> GetGuestById(int id);

        Task<IBaseResponse<bool>> DeleteGuest(int id);

        Task<IBaseResponse<bool>> UpdateGuest(Guest model);
        
        Task<IBaseResponse<bool>> DublicateGuest(string str1, string str2);
    }
}
