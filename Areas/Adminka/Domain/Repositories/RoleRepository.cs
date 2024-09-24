using OfficePass.Domain.Entities;
using OfficePass.Domain;

namespace OfficePass.Areas.Adminka.Domain.Repositories
{
    public class RoleRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public RoleRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<Role> GetAll()
        {
            return dbcontext.Roles;
        }
         
    }
}
