using OfficePass.Domain.Entities;

namespace OfficePass.Domain.Repositories
{
    public class LoginRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public LoginRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<User> GetAll()
        {
            return dbcontext.Users;
        }

    }
}
