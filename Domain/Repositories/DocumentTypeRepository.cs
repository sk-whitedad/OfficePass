using OfficePass.Domain.Entities;

namespace OfficePass.Domain.Repositories
{
    public class DocumentTypeRepository
    {
        private readonly OfficepassdbContext dbcontext;

        public DocumentTypeRepository(OfficepassdbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            this.dbcontext = dbcontext;
        }

        public IQueryable<DocumentType> GetAll()
        {
            return dbcontext.DocumentTypes;
        }

    }
}
