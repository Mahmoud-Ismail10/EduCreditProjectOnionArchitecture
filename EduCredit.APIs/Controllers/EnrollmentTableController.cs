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
        private readonly ISemesterServices _semesterServices;

        public EnrollmentTableController(IEnrollmentTableServices enrollmentTableServices)
        {
            _enrollmentTableServices = enrollmentTableServices;
        }

        [HttpGet("GetEnrollOfCourse")]
        [Authorize(Roles = $"{AuthorizationConstants.StudentRole}, {AuthorizationConstants.SuperAdminRole}")]
        public async Task<ActionResult<IReadOnlyList<ReadEnrollmentTableDto>>> GetStudentWithHisAvalaibleCourses()
        {
           
            var userId = User.FindFirstValue("userId");
            var studentCourses = await _enrollmentTableServices.GetStudentAvailableCourses(userId);
            if (studentCourses is null) return NotFound(new ApiResponse(404));
            return Ok(studentCourses);
        }

        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}, {AuthorizationConstants.StudentRole}")]
        public async Task<ActionResult> CreateEnrollmentTable(CreateOrUpdateEnrollmentTableDto createOrUpdateEnrollmentTableDto)
        {
            var userId = User.FindFirstValue("userId");
            var response = await _enrollmentTableServices.CreateOrUpdateEnrollmentTable(createOrUpdateEnrollmentTableDto, userId);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, response.ErrorMessage));
            return BadRequest(new ApiResponse(400, response.ErrorMessage));
        }
    }
}
