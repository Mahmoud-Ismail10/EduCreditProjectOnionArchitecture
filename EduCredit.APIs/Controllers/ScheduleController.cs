using Azure;
using EduCredit.Core.Security;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult<ApiResponse>> AssignTeacherToCourse([FromBody] CreateScheduleDto createScheduleDto)
        {
            var response = await _scheduleServices.AssignSchedule(createScheduleDto);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, response.ErrorMessage));
            return BadRequest(new ApiResponse(400, response.ErrorMessage));
        }

        [HttpGet("{courseId}/{teacherId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ReadScheduleDto>> GetSchedule(Guid courseId, Guid teacherId)
        {
            var scheduleDto = await _scheduleServices.GetSchedule(courseId, teacherId);
            if (scheduleDto is null)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            return Ok(scheduleDto);
        }

        [HttpPut("{courseId}/{teacherId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateSchedule(Guid courseId, Guid teacherId, [FromBody] UpdateScheduleDto updateScheduleDto)
        {
            var response = await _scheduleServices.UpdateSchedule(courseId, teacherId, updateScheduleDto);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Schedule updated successfully!"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to update the schedule!"));
        }
        
        [HttpDelete("{courseId}/{teacherId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteSchedule(Guid courseId, Guid teacherId)
        {
            var response = await _scheduleServices.DeleteSchedule(courseId, teacherId);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Schedule deleted successfully!"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Schedule not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the schedule!"));
        }

    } 
}
