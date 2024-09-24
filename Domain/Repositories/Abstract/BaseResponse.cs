using OfficePass.Enums;

namespace OfficePass.Domain.Repositories.Abstract
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }

        public StatusCode StatusCode { get; set; }

        public T Data { get; set; }

        
    }
}
