using EduCredit.Core.Enums;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.SemesterSpecifications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
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
                    return Ok(new ApiResponse(200, response.Message));
                return BadRequest(new ApiResponse(400, response.Message));
            }
            return BadRequest(new ApiResponse(400, "Invalid input data!"));
        }

        /// PUT: api/Semester/CurrentSemester
        [HttpGet("CurrentSemester")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<ReadSemesterDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse<ReadSemesterDto>>> GetCurrentSemester()
        {
            var response = await _semesterServices.GetCurrentSemester();
            if (response is null)
                return NotFound(new ApiResponse(404, "Current semester not found!"));
            return Ok(new ApiResponse<ReadSemesterDto>(200, "Current semester retrieved successfully", response));
        }

        /// GET: api/Semester
        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<IReadOnlyList<ReadSemesterDto>>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<ApiResponse<Pagination<IReadOnlyList<ReadSemesterDto>>>> GetAllSemesters([FromQuery]SemesterSpecificationParams spec)
        {
            int count;
            var semesters = _semesterServices.GetAllSemesters(spec,out count);
            if (semesters is null)
                return NotFound(new ApiResponse(404, "No semesters found!"));
            return Ok(new ApiResponse<Pagination<ReadSemesterDto>>(200, "Success", new Pagination<ReadSemesterDto>(spec.PageSize, spec.PageIndex, count, semesters)));
        }

        /// GET: api/Semester/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<ReadSemesterDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponse<ReadSemesterDto>>> GetSemesterById(Guid id)
        {
            var response = await _semesterServices.GetSemesterByIdAsync(id);
            if (response is null)
                return NotFound(new ApiResponse(404, "Semester not found!"));
            return Ok(new ApiResponse<ReadSemesterDto>(200, "Success", response));
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
