using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories.Abstract;

namespace OfficePass.Services
{
    public interface IDocumentTypeService
    {
        Task<IBaseResponse<List<DocumentType>>> GetDocumentTypes();
    }
}
