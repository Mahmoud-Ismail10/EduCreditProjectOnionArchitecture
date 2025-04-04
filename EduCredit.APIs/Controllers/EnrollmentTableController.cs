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
        [Authorize(Roles = $"{AuthorizationConstants.StudentRole}, {AuthorizationConstants.SuperAdminRole}")]
        public async Task<ActionResult<IReadOnlyList<ReadEnrollmentTableDto>>> GetStudentWithHisAvalaibleCourses([FromQuery] string studentId)
        {
            var currentUserId = User.FindFirstValue("userId");
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == AuthorizationConstants.StudentRole)
                studentId = currentUserId;
            var studentCourses = await _enrollmentTableServices.GetStudentAvailableCourses(studentId);
            if (studentCourses is null) return NotFound(new ApiResponse(404));
            return Ok(studentCourses);
        }

        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}, {AuthorizationConstants.StudentRole}")]
        public async Task<ActionResult> CreateEnrollmentTable(CreateOrUpdateEnrollmentTableDto createOrUpdateEnrollmentTableDto)
        {
            var currentUserIdAsString = User.FindFirstValue("userId");
            var currentUserIdAsGuid = Guid.Parse(currentUserIdAsString);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == AuthorizationConstants.StudentRole)
                createOrUpdateEnrollmentTableDto.StudentId = currentUserIdAsGuid;
            var response = await _enrollmentTableServices.CreateOrUpdateEnrollmentTable(createOrUpdateEnrollmentTableDto);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, response.Message));
                return BadRequest(new ApiResponse(400, response.Message));
        }
    }
}
