namespace OfficePass.Enums
{
    public enum StatusCode
    {
        NotFound = 0,
        AlreadyExists = 1,
        IncorrectPassword = 2,
        NotCreateObject = 3,
        UpdateDBError = 4,
        IsExists = 5, //когда запись уже существует
        OK = 200,
        InternalServerError = 500
    }
}
