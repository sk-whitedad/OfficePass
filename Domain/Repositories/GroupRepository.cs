using Microsoft.EntityFrameworkCore;
using OfficePass.Domain.Entities;

namespace OfficePass.Domain.Repositories
{
    public class GroupRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public GroupRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<Group> GetAll()
        {
            return dbcontext.Groups;
        }

        public async Task Create(Group entity)
        {
            dbcontext.Groups.Add(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(Group entity)
        {
            dbcontext.Groups.Remove(entity);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Update(Group entity)
        {
            var model = await dbcontext.Groups.FirstOrDefaultAsync(x => x.Id == entity.Id);
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
