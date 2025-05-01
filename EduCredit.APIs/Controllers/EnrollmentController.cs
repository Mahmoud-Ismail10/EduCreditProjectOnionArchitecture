using EduCredit.Core.Relations;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class EnrollmentController : BaseApiController
    {
        private readonly IEnrollmentServices _enrollmentServices;

        public EnrollmentController(IEnrollmentServices enrollmentServices)
        {
            _enrollmentServices = enrollmentServices;
        }

        [HttpPut("{enrollmentTableId}/{courseId}")]
        [Authorize(Roles = $"{AuthorizationConstants.TeacherRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> AssignOrUpdateGrade(Guid enrollmentTableId, Guid courseId, [FromBody] UpdateEnrollmentDto updateEnrollmentDto)
        {
            var response = await _enrollmentServices.AssignOrUpdateGrade(enrollmentTableId, courseId, updateEnrollmentDto);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "The grade was successfully assigned!"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Enrollment not found!"));
            else
                return BadRequest(new ApiResponse(400, "Failure to assign the grade!"));
        }
        [HttpGet("Results/{studentId}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole},{AuthorizationConstants.TeacherRole},{AuthorizationConstants.StudentRole}")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<ReadEnrollmentDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public ActionResult<ApiResponse<Pagination<EnrollmentResultDto>>> GetAllResults([FromQuery] EnrollmentSpecificationParams spec,Guid studentId)
        {
            int count;
            var Enrollments = _enrollmentServices.GetAllEnrollmentsAsync(spec,studentId,out count);
            if (Enrollments is null)
                return NotFound(new ApiResponse(404, "Enrollment not found!"));
            return Ok(new ApiResponse<EnrollmentResultDto>(200, "Success", Enrollments));
        }

        //[HttpGet("Results")]
        //[ProducesResponseType(typeof(ApiResponse<Pagination<ReadEnrollmentDto>>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        //public ActionResult<ApiResponse<Pagination<IReadOnlyList<Enrollment>>>> GetStudentEnrollment([FromQuery] EnrollmentSpecificationParams spec)
        //{


            #region oldVersion
            //[HttpPost]
            //[Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}, {AuthorizationConstants.StudentRole}")]
            //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
            //public async Task<ActionResult<ApiResponse>> AssignCourseToEnrollmentTable([FromBody] CreateEnrollmentDto createEnrollmentDto)
            //{
            //    var response = await _enrollmentServices.AssignEnrollment(createEnrollmentDto);
            //    if (response.StatusCode == 200)
            //        return NoContent();
            //    return BadRequest(new ApiResponse(400, response.Message));
            //}

            //[HttpDelete("{enrollmentTableId}/{courseId}")]
            //[Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}, {AuthorizationConstants.StudentRole}")]
            //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
            //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
            //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
            //public async Task<ActionResult<ApiResponse>> DeleteEnrollment(Guid enrollmentTableId, Guid courseId)
            //{
            //    var response = await _enrollmentServices.DeleteEnrollment(enrollmentTableId, courseId);
            //    if (response.StatusCode == 200)
            //        return Ok(new ApiResponse(200, "Enrollment deleted successfully!"));
            //    else if (response.StatusCode == 404)
            //        return NotFound(new ApiResponse(404, "Enrollment not found!"));
            //    else
            //        return BadRequest(new ApiResponse(400, "It is not suitable to delete the enrollmet!"));
            //}

            //[HttpGet("{enrollmentTableId}/{courseId}")]
            //[Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
            //[ProducesResponseType(typeof(ApiResponse<ReadEnrollmentDto>), (int)HttpStatusCode.OK)]
            //[ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
            //public async Task<ActionResult<ApiResponse<ReadEnrollmentDto>>> GetEnrollment(Guid enrollmentTableId, Guid courseId)
            //{
            //    var enrollmentDto = await _enrollmentServices.GetEnrollment(enrollmentTableId, courseId);
            //    if (enrollmentDto is null)
            //        return NotFound(new ApiResponse(404, "Enrollment not found!"));
            //    return Ok(new ApiResponse<ReadEnrollmentDto>(200,"Success",enrollmentDto));
            //}
            #endregion
        }
}
