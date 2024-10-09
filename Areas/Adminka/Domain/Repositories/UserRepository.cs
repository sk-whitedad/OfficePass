using Microsoft.EntityFrameworkCore;
using OfficePass.Areas.Adminka.Services;
using OfficePass.Domain;
using OfficePass.Domain.Entities;

namespace OfficePass.Areas.Adminka.Domain.Repositories
{
    public class UserRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public UserRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<User> GetAll()
        {
            return dbcontext.Users;
        }

        public async Task Create(User user)
        {
            dbcontext.Users.Add(user);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            dbcontext.Users.Remove(user);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Update(User entity)
        {
            var user = await dbcontext.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (user != null)
            {
                user.Login = entity.Login;
                user.Password = entity.Password;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
