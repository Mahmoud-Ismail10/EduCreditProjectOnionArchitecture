using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Helper
{
    public class IsPeriodEndedAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var semesterService = context.HttpContext.RequestServices.GetRequiredService<ISemesterServices>();
            var currentSemester = await semesterService.GetCurrentSemester();

            if (currentSemester is null)
            {
                context.Result = new NotFoundObjectResult(new ApiResponse(400, "There is no current semester!"));
                return;
            }
            else
            {
                if (currentSemester.EnrollmentClose < DateTime.UtcNow)
                {
                    context.Result = new BadRequestObjectResult(new ApiResponse(400, "The enrollment period ended!"));
                    return;
                }
                else if (currentSemester.EnrollmentOpen > DateTime.UtcNow)
                {
                    context.Result = new BadRequestObjectResult(new ApiResponse(400, "The enrollment period has not started yet!"));
                    return;
                }
                else
                    await next();
            }
        }
    }
}
