using AutoMapper;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class TeacherController : BaseApiController
    {
        private readonly ITeacherServices _teacherServices;

        public TeacherController(ITeacherServices teacherServices)
        {
            _teacherServices = teacherServices;
        }

        /// GET: api/Teachers
        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(Pagination<ReadTeacherDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<IReadOnlyList<ReadTeacherDto>> GetAllTeachers([FromQuery] TeacherSpecificationParams specParams) // Create class contains all of params (refactor)
        {
            int count;
            var teachersDto = _teacherServices.GetAllTeachers(specParams, out count);
            if (teachersDto is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadTeacherDto>(specParams.PageSize, specParams.PageIndex, count, teachersDto)); // Status code = 200
        }

        /// GET: api/Teacher/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ReadTeacherDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReadTeacherDto>> GetTeacher(Guid id)
        {
            var teacherDto = await _teacherServices.GetTeacherByIdAsync(id);
            if (teacherDto is null) return NotFound(new ApiResponse(404));
            return Ok(teacherDto);
        }

        /// PUT: api/Teacher/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(UpdateTeacherDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UpdateTeacherDto>> UpdateTeacher([FromBody] UpdateTeacherDto updateteachertDto, Guid id)
        {
            var teacherDto = await _teacherServices.UpdateTeacherAsync(updateteachertDto, id);
            if (teacherDto is null) return NotFound(new ApiResponse(404));
            return Ok(teacherDto);
        }

        /// DELETE: api/Teacher/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteTeacher(Guid id)
        {
            var response = await _teacherServices.DeleteTeacherAsync(id);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Teacher deleted successfully"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Teacher not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the Teacher!"));
        }
    }
}
