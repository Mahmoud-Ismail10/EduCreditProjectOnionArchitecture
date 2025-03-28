using System.Text.Json.Serialization;

namespace EduCredit.Service.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int _StatusCode, string? _ErrorMessage = null)
        {
            StatusCode = _StatusCode;
           Message = _ErrorMessage ?? GetErrorMessage(StatusCode);
        }

        private string? GetErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request!",
                401 => "UnAutherized!",
                404 => "Not Found!",
                500 => "Server Error",
                _ => null
            };
        }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public T? Result { get; set; }

        public ApiResponse(int statusCode, string message, T? result = default)
            : base(statusCode, message)
        {
            Result = result;
        }
    }
}
