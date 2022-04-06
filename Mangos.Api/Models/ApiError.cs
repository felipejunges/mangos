namespace Mangos.Api.Models
{
    public class ApiError
    {
        public string Message { get; private set; }

        public ApiError(string message)
        {
            Message = message;
        }
    }
}