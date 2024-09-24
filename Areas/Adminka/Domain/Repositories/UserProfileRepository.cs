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
            var userProfile = await dbcontext.UserProfiles.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (userProfile != null)
            {
                userProfile.Firstname = entity.Firstname;
                userProfile.Lastname = entity.Lastname;
                userProfile.Surname = entity.Surname;
                userProfile.Group = entity.Group;
                userProfile.Specialization = entity.Specialization;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
