using Microsoft.EntityFrameworkCore;
using OfficePass.Domain.Entities;

namespace OfficePass.Domain.Repositories
{
    public class SpecializationRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public SpecializationRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<Specialization> GetAll()
        {
            return dbcontext.Specializations;
        }

        public async Task Create(Specialization entity)
        {
            dbcontext.Specializations.Add(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(Specialization entity)
        {
            dbcontext.Specializations.Remove(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Update(Specialization entity)
        {
            var model = await dbcontext.Specializations.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (model != null)
            {
                model.Name = entity.Name;
                model.Description = entity.Description;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
