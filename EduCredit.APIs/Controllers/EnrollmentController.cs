using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> AssignCourseToEnrollmentTable([FromBody] CreateEnrollmentDto createEnrollmentDto)
        {
            var response = await _enrollmentServices.AssignEnrollment(createEnrollmentDto);
            if (response.StatusCode == 200)
                return NoContent();
            return BadRequest(new ApiResponse(400, "Failure to assign the enrollment!"));
        }
        
        [HttpDelete("{enrollmentTableId}/{courseId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteEnrollment(Guid enrollmentTableId, Guid courseId)
        {
            var response = await _enrollmentServices.DeleteEnrollment(enrollmentTableId, courseId);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Enrollment deleted successfully!"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Enrollment not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the enrollmet!"));
        }

    }
}
