using Microsoft.EntityFrameworkCore;
using OfficePass.Domain.Entities;

namespace OfficePass.Domain.Repositories
{
    public class GuestRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public GuestRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<Guest> GetAll()
        {
            return dbcontext.Guests;
        }

        public async Task Create(Guest entity)
        {
            dbcontext.Guests.Add(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(Guest entity)
        {
            dbcontext.Guests.Remove(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Update(Guest entity)
        {
            var guest = await dbcontext.Guests.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (guest != null)
            {
                guest.FirstName = entity.FirstName;
                guest.LastName = entity.LastName;
                guest.SurName = entity.SurName;
                guest.DocumentSerial = entity.DocumentSerial;
                guest.DocumentNumber = entity.DocumentNumber;
                guest.DocumentTypeId = entity.DocumentTypeId;
                guest.CompanyId = entity.CompanyId;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
