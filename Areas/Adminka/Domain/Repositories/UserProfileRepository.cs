using OfficePass.Domain.Entities;
using OfficePass.Domain;
using Microsoft.EntityFrameworkCore;

namespace OfficePass.Areas.Adminka.Domain.Repositories
{
    public class UserProfileRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public UserProfileRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<UserProfile> GetAll()
        {
            return dbcontext.UserProfiles;
        }

        public async Task Create(UserProfile entity)
        {
                await dbcontext.UserProfiles.AddAsync(entity);
                await dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Update(UserProfile entity)
        {
            var model = await dbcontext.UserProfiles.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (model != null)
            {
                model.Firstname = entity.Firstname;
                model.Lastname = entity.Lastname;
                model.Surname = entity.Surname;
                model.Group = entity.Group;
                model.GroupId = entity.GroupId;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task Delete(UserProfile entity)
        {
            dbcontext.UserProfiles.Remove(entity);
            await dbcontext.SaveChangesAsync();
        }
    }
}
