namespace EduCredit.APIs.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode, string? ErrorMessage = null, string? _Details = null) : base(StatusCode, ErrorMessage)
        {
            Details = _Details;
        }
    }
}
