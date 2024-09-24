using Microsoft.EntityFrameworkCore;
using OfficePass.Domain.Entities;

namespace OfficePass.Domain.Repositories
{
    public class CompanyRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public CompanyRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<Company> GetAll()
        {
            return dbcontext.Companies;
        }

        public async Task Create(Company company)
        {
            dbcontext.Companies.Add(company);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(Company company)
        {
            dbcontext.Companies.Remove(company);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Update(Company company)
        {
            var user = await dbcontext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
            if (user != null)
            {
                user.Name = company.Name;
                user.Address = company.Address;
                user.PhoneNumber = company.PhoneNumber;
                await dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
