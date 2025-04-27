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
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(ApiResponse<CreateCourseDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<CreateCourseDto>>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _courseServices.CreateCourseAsync(createCourseDto);
                if (response != null)
                      return Ok(new ApiResponse<CreateCourseDto>(201, "Course Created Successfully", response));
             
                return BadRequest(new ApiResponse(400, "Failed to create course!"));
            }
            return BadRequest(new ApiResponse(400, "Invalid data!"));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ReadCourseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<ReadCourseDto>>> GetCourse(Guid id)
        {
            var courseDto = await _courseServices.GetCourseByIdAsync(id);
            if (courseDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<ReadCourseDto>(200,"Success", courseDto));
        }
        [HttpGet("{teacherId}/courses")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ReadTeacherCourseDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ReadTeacherCourseDto>>>> GetCoursesByTeacherId(Guid teacherId)
        {

            var courses = await _courseServices.GetCoursesByTeacherIdAsync(teacherId);
            if (courses is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<IReadOnlyList<ReadTeacherCourseDto>>(200, "Success", courses));
        }

        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Pagination<ReadCourseDto>), (int)HttpStatusCode.OK)]
        public ActionResult<IReadOnlyList<ReadCourseDto>> GetCourses([FromQuery] CourseSpecificationParams specParams)
        {
            int count;
            var coursesDto = _courseServices.GetAllCourses(specParams, out count);
            if (coursesDto is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadCourseDto>(specParams.PageSize, specParams.PageIndex, count, coursesDto));
        }
       // [HttpGet("GetCoursesInCurrentSemester")]
       //// [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
       // [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
       // [ProducesResponseType(typeof(Pagination<ReadCourseDto>), (int)HttpStatusCode.OK)]
       // public async Task<ActionResult<IReadOnlyList<ReadCourseDto>>> GetCoursesInCurrentSemester([FromQuery] CourseSpecificationParams specParams)
       // {
       //     var coursesDto =await _courseServices.GetCoursesByCurrentSemesterId(specParams.DepartmentId);
       //     if (coursesDto is null) return NotFound(new ApiResponse(404));
       //     return Ok(new Pagination<ReadCourseDto>(specParams.PageSize, specParams.PageIndex,coursesDto.Count, coursesDto));
       // }


        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReadCourseDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse>> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _courseServices.UpdateCourseAsync(updateCourseDto, id);
                if (response.StatusCode == 200)
                    return Ok(new ApiResponse(200, "Course updated successfully!"));
                else if (response.StatusCode == 404)
                    return NotFound(new ApiResponse(404, "Course not found!"));
                else
                    return BadRequest(new ApiResponse(400, "It is not suitable to update the course!"));
            }
            return BadRequest(new ApiResponse(400, "Invalid data!"));
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
