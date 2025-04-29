using Azure;
using EduCredit.Core.Models;
using EduCredit.Core.Security;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace EduCredit.APIs.Controllers
{
    public class ScheduleController : BaseApiController
    {
        private readonly IScheduleServices _scheduleServices;

        public ScheduleController(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }

        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> AssignTeachersToCourse([FromBody] CreateScheduleDto createScheduleDto)
        {
            var response = await _scheduleServices.AssignSchedule(createScheduleDto);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, response.Message));
            return BadRequest(new ApiResponse(400, response.Message));
        }

        /// GET: api/Schedule/{CourseId}/{SemesterId}
        [HttpGet("{CourseId}/{SemesterId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<ReadScheduleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse<ReadScheduleDto>>> GetSchedule(Guid CourseId, Guid SemesterId)
        {
            var scheduleDto = await _scheduleServices.GetSchedule(CourseId, SemesterId);
            if (scheduleDto is null)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            return Ok(new ApiResponse<ReadScheduleDto>(200, "Success", scheduleDto));
        }

        /// GET: api/Schedule/Study-Schedule/{StudentId}
        [HttpGet("Study-Schedule/{StudentId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ReadScheduleEnrollCourseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ReadScheduleEnrollCourseDto>>>> GetSchedulesByStudentId(Guid StudentId)
        {
            var schedulesDto = await _scheduleServices.GetSchedulesByStudentId(StudentId);
            if (schedulesDto is null)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            return Ok(new ApiResponse<IReadOnlyList<ReadScheduleEnrollCourseDto>>(200, "Success", schedulesDto));
        }

        /// GET: api/Schedule/Get-EnrollmentOfCourseScheduale/{StudentId}
        [HttpGet("Get-EnrollmentOfCourseScheduale/{StudentId}")]
        [Authorize(Roles = $"{AuthorizationConstants.StudentRole}, {AuthorizationConstants.SuperAdminRole}")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ReadScheduleEnrollCourseDto>>>> GetStudentWithHisAvalaibleCourses(Guid StudentId)
        {
            var currentUserIdAsString = User.FindFirstValue("userId");
            var currentUserIdAsGuid = Guid.Parse(currentUserIdAsString);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == AuthorizationConstants.StudentRole)
                StudentId = currentUserIdAsGuid;
            var studentCourses = await _scheduleServices.GetStudentWithAvailableCourses(StudentId);
            if (studentCourses is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<IReadOnlyList<ReadScheduleEnrollCourseDto>>(200, "Success", studentCourses));
        }

        /// PUT: api/Schedule/{CourseId}
        [HttpPut("{CourseId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateSchedule(Guid CourseId, [FromBody] UpdateScheduleDto updateScheduleDto)
        {
            var response = await _scheduleServices.UpdateSchedule(CourseId, updateScheduleDto);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Schedule updated successfully!"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to update the schedule!"));
        }

        /// DELETE: api/Schedule/{CourseId}
        [HttpDelete("{CourseId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteSchedule(Guid CourseId)
        {
            var response = await _scheduleServices.DeleteSchedule(CourseId);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Schedule deleted successfully!"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the schedule!"));
        }
    }
}
