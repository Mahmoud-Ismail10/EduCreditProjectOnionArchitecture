using EduCredit.Core.Models;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class CourseController : BaseApiController
    {
        private readonly ICourseServices _courseServices;

        public CourseController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            if (ModelState.IsValid)
            {
                var courseDto = await _courseServices.CreateCourseAsync(createCourseDto);
                if (courseDto is not null)
                    return Ok(new ApiResponse(400));
               return Ok(new ApiResponse(200,"Course Added successfully"));
            }
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ReadCourseDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ReadCourseDto>> GetCourse(Guid id)
        {
            var courseDto = await _courseServices.GetCourseByIdAsync(id);
            if (courseDto is null) return NotFound(new ApiResponse(404));
            return Ok(courseDto);
        }

        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ReadCourseDto), (int)HttpStatusCode.OK)]
        public ActionResult<IReadOnlyList<ReadCourseDto>> GetCourses([FromQuery] CourseSpecificationParams specParams)
        {
            int count;
            var coursesDto = _courseServices.GetAllCourses(specParams, out count);
            if (coursesDto is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadCourseDto>(specParams.PageSize, specParams.PageIndex, count, coursesDto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReadCourseDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateCourseDto>> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
        {
            if (ModelState.IsValid)
            {
                var courseDto = await _courseServices.UpdateCourseAsync(updateCourseDto, id);
                if (courseDto is null) return NotFound(new ApiResponse(404));
                return Ok(courseDto);
            }
            return BadRequest(new ApiResponse(400));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            var response = await _courseServices.DeleteCourseAsync(id);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Course deleted successfully"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Course not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the course!"));
        }
    }
}
