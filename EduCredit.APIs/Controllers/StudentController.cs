using Azure;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class StudentController : BaseApiController
    {
        private readonly IStudentServices _studentServices;

        public StudentController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }

        /// GET: api/Students
        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(Pagination<ReadTeacherDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<IReadOnlyList<ReadStudentDto>> GetAllStudents([FromQuery] StudentSpecificationParams specParams)
        {
            int count;
            var students = _studentServices.GetAllStudents(specParams, out count);
            if (students is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadStudentDto>(specParams.PageSize, specParams.PageIndex, count, students));
        }

        /// GET: api/Students/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ReadTeacherDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReadStudentDto>> GetStudent(Guid id)
        {
            var student = await _studentServices.GetStudentByIdAsync(id);
            if (student is null) return NotFound(new ApiResponse(404));
            return Ok(student);
        }

        /// PUT: api/Students/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse>> UpdateStudent(Guid id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _studentServices.UpdateStudentAsync(updateStudentDto, id);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, "Student updated successfully"));
                else if (response.StatusCode == 404)
                    return NotFound(new ApiResponse(404, "Student not found!"));
                else
                    return BadRequest(new ApiResponse(400, "It is not suitable to update the Student!"));
            }
            return BadRequest(new ApiResponse(400));
        }

        /// DELETE: api/Students/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReadStudentDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse>> DeleteStudent(Guid id)
        {
            var response = await _studentServices.DeleteStudnetAsync(id);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Student deleted successfully"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Student not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the Student!"));
        }


    }
}
