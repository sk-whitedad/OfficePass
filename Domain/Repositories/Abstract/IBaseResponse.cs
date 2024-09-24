using Microsoft.AspNetCore.Http;
using OfficePass.Enums;

namespace OfficePass.Domain.Repositories.Abstract
{
    public interface IBaseResponse<T>
    {
        string Description { get; }
        StatusCode StatusCode { get; }
        T Data { get; }
    }
}
