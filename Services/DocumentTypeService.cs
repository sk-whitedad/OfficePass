using OfficePass.Domain.Entities;
using OfficePass.Domain.Repositories;
using OfficePass.Domain.Repositories.Abstract;
using OfficePass.Enums;

namespace OfficePass.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly DocumentTypeRepository documentTypeRepository;

        public DocumentTypeService(DocumentTypeRepository documentTypeRepository)
        {
            if (documentTypeRepository == null) throw new ArgumentNullException(nameof(documentTypeRepository));
            this.documentTypeRepository = documentTypeRepository;
        }

        public async Task<IBaseResponse<List<DocumentType>>> GetDocumentTypes()
        {
            try
            {
                var document = documentTypeRepository.GetAll().ToList();
                if (document == null)
                {
                    return new BaseResponse<List<DocumentType>>
                    {
                        Description = "Документы не найдены",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<DocumentType>>
                {
                    Data = document,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<DocumentType>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
