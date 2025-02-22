namespace EduCredit.APIs.Errors
{
    public class ApiResponse
    {
       // public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public ApiResponse(int _StatusCode, string? _ErrorMessage = null)
        {
           // StatusCode = _StatusCode;
            ErrorMessage = _ErrorMessage ?? GetErrorMessage(_StatusCode);
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
}
