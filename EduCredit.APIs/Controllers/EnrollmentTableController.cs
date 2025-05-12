using EduCredit.Core.Security;
using EduCredit.Service.DTOs.CourseDTOs;
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

        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}, {AuthorizationConstants.AdminRole}, {AuthorizationConstants.StudentRole}")]
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

        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}, {AuthorizationConstants.AdminRole}, {AuthorizationConstants.StudentRole}")]
        public async Task<ActionResult> GetEnrollmentTable([FromQuery] Guid StudentId)
        {
            var enrollmentTableDto = await _enrollmentTableServices.GetEnrollmentTableByStudentId(StudentId);
            if (enrollmentTableDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<CreateOrUpdateEnrollmentTableDto>(200, "Success", enrollmentTableDto));
        }

        [HttpPatch]
        [Authorize(Roles = $"{AuthorizationConstants.TeacherRole}")]
        public async Task<ActionResult<ApiResponse>> UpdateEnrollmentTableStatus([FromQuery] Guid EnrollmentTableId, [FromBody] UpdateEnrollmentTableDto dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _enrollmentTableServices.UpdateEnrollmentTableStatus(EnrollmentTableId, dto);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, response.Message));
                else if (response.StatusCode == 404)
                    return NotFound(new ApiResponse(404, response.Message));
                else if (response.StatusCode == 400)
                    return BadRequest(new ApiResponse(400, response.Message));
            }
            return BadRequest(new ApiResponse(400, "Status is required!"));

        }
    }
}
