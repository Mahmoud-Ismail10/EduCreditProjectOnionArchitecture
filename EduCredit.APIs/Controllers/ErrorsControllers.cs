using EduCredit.Service.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduCredit.APIs.Controllers
{
    [Route("Error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // When you have private end-point and don't want to do it documentation
    public class ErrorsControllers : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                StatusCodes.Status401Unauthorized => Unauthorized(new ApiResponse(code)),
                StatusCodes.Status404NotFound => NotFound(new ApiResponse(code)),
                _ => StatusCode(code)
            };
        }
    }
}
