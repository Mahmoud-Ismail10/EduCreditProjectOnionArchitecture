using EduCredit.Core.Security;
using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCredit.APIs.Controllers
{
   
    public class EnrollmentTableController : BaseApiController
    {
        private readonly IEnrollmentTableServices _enrollmentTableServices;

        public EnrollmentTableController(IEnrollmentTableServices enrollmentTableServices)
        {
            _enrollmentTableServices = enrollmentTableServices;
        }
        [HttpGet("GetEnrollOfCourse")]
        [Authorize(Roles = AuthorizationConstants.StudentRole)]
        public async Task<ActionResult<IReadOnlyList<ReadEnrollmentTableDto>>> GetStudentWithHisAvalaibleCourses()
        {
           
            var userId =User.FindFirstValue("userId");
            var studentCourses = await _enrollmentTableServices.GetStudentAvailableCourses(userId);
            if (studentCourses is null) return NotFound(new ApiResponse(404));
            return Ok(studentCourses);
        }
    }
}
