using Azure;
using EduCredit.Core.Models;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.ScheduleSpecifications;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
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

        /// POST: api/Schedule
        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> AssignSchedule([FromBody] CreateScheduleDto createScheduleDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _scheduleServices.AssignSchedule(createScheduleDto);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, response.Message));
                return BadRequest(new ApiResponse(400, response.Message));
            }
            return BadRequest(new ApiResponse(400, "Invalid input data!"));
        }

        /// GET: api/Schedule/{CourseId}/{SemesterId}
        [HttpGet("{CourseId}/{SemesterId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ReadScheduleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponse<ReadScheduleDto>>> GetSchedule(Guid CourseId, Guid SemesterId)
        {
            var scheduleDto = await _scheduleServices.GetSchedule(CourseId, SemesterId);
            if (scheduleDto is null)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            return Ok(new ApiResponse<ReadScheduleDto>(200, "Success", scheduleDto));
        }

        /// GET: api/Schedule/{CourseId}
        [HttpGet("{CourseId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<ReadScheduleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponse<ReadScheduleDto>>> GetSchedule(Guid CourseId)
        {
            var scheduleDto = await _scheduleServices.GetSchedule(CourseId);
            if (scheduleDto is null)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            return Ok(new ApiResponse<ReadScheduleDto>(200, "Success", scheduleDto));
        }

        /// GET: api/Schedule
        [HttpGet()]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}, {AuthorizationConstants.AdminRole}, {AuthorizationConstants.TeacherRole}")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ReadScheduleDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<ApiResponse<ReadScheduleDto>> GetAllSchedules([FromQuery] ScheduleSpecificationParams specParams)
        {
            int count;
            var schedulesDto = _scheduleServices.GetAllSchedules(specParams, out count);
            if (schedulesDto is null)
                return NotFound(new ApiResponse(404, "There is no schedules!"));
            return Ok(new Pagination<ReadScheduleDto>(specParams.PageSize, specParams.PageIndex, count, schedulesDto));
        }

        /// GET: api/Schedule/Study-Schedule/{StudentId}
        [HttpGet("Study-Schedule/{StudentId}")]
        [Authorize]
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
        [IsPeriodEnded]
        [HttpGet("Get-EnrollmentOfCourseScheduale/{StudentId}")]
        [Authorize]
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
                return BadRequest(new ApiResponse(400, response.Message));
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
