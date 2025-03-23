using EduCredit.Core.Enums;
using EduCredit.Core.Security;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class SemesterController : BaseApiController
    {
        private readonly ISemesterServices _semesterServices;

        public SemesterController(ISemesterServices semesterServices)
        {
            _semesterServices = semesterServices;
        }

        /// POST: api/Semester/AssignCoursesToSemester
        [HttpPost("AssignCoursesToSemester")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AssignCoursesToSemester([FromBody] CreateSemesterDto createSemesterDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _semesterServices.CreateSemester(createSemesterDto);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, response.ErrorMessage));
                return BadRequest(new ApiResponse(400, response.ErrorMessage));
            }
            return BadRequest(new ApiResponse(400, "Invalid input data!"));
        }

        /// PUT: api/Semester/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateSemester(Guid id, [FromBody] UpdateSemesterDto updateSemesterDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _semesterServices.UpdateSemester(updateSemesterDto, id);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, "Semester updated successfully"));
                return BadRequest(new ApiResponse(400, "Unable to update semester!"));
            }
            return BadRequest(new ApiResponse(400, "Invalid input data!"));
        }

        /// DELETE: api/Semester/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteSemester(Guid id)
        {
            var response = await _semesterServices.DeleteSemester(id);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Semester deleted successfully"));
            else if (response.StatusCode == 400)
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the Semester!"));
            else
                return NotFound(new ApiResponse(404, "Semester not found!"));
        }

    }
}
