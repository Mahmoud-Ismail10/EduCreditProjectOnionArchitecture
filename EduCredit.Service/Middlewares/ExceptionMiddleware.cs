using EduCredit.Service.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace EduCredit.Service.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionResponse> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionResponse> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _next.Invoke(httpcontext); // for Development
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                /// Manage Header of Response
                httpcontext.Response.ContentType = "application/json";
                httpcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                /// Manage Body of Response
                var response = _env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                #region PascalCase vs CamelCase
                /// Pascal Case: The first letter of each word is capitalized, including the first word.
                /// Camel Case: The first letter of the first word is lowercase, and the first letter of each subsequent word is capitalized. 
                #endregion
                // Set parameters of Response in CamelCase
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(response, options);
                await httpcontext.Response.WriteAsync(json);
            }
        }
    }
}
