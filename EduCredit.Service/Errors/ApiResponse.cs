using System.Text.Json.Serialization;

namespace EduCredit.Service.Errors
{
    public class ApiResponse
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public ApiResponse(int _StatusCode, string? _ErrorMessage = null)
        {
            StatusCode = _StatusCode;
            ErrorMessage = _ErrorMessage ?? GetErrorMessage(StatusCode);
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
            public T? Data { get; set; }

            public ApiResponse(int statusCode, string message, T? data = default)
                : base(statusCode, message)
            {
                Data = data;
            }
        }
}
