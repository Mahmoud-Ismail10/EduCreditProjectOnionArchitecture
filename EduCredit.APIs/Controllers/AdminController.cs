using EduCredit.Core.Enums;
using EduCredit.Core.Security;
using EduCredit.Core.Specifications.AdminSpecifications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        /// GET: api/Admin
        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<ReadAdminDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<ApiResponse<Pagination<IReadOnlyList<ReadAdminDto>>>> GetAllAdmins([FromQuery] AdminSpecificationParams specParams) // Create class contains all of params (refactor)
        {
            int count;
            var AdminsDto = _adminServices.GetAllAdmins(specParams, out count);
            if (AdminsDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<Pagination<ReadAdminDto>>(200,"Success",new Pagination<ReadAdminDto>(specParams.PageSize, specParams.PageIndex, count, AdminsDto))); // Status code = 200
        }

        /// GET: api/Admin/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ReadAdminDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponse<ReadAdminDto>>> GetAdmin(Guid id)
        {
            var AdminDto = await _adminServices.GetAdminByIdAsync(id);
            if (AdminDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<ReadAdminDto>(200,"Success",AdminDto));
        }

        /// PUT: api/Admin/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponse>> UpdateAdmin([FromBody] UpdateAdminDto updateAdmintDto, Guid id)
        {
            var AdminDto = await _adminServices.UpdateAdminAsync(updateAdmintDto, id);
            if (AdminDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse(200,"Success"));
        }

        /// DELETE: api/Admin/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteAdmin(Guid id)
        {
            var response = await _adminServices.DeleteAdminAsync(id);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Admin deleted successfully"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Admin not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the Admin!"));
        }
        [HttpGet("statistics/{type}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole},{AuthorizationConstants.TeacherRole}")]
        public async Task<ActionResult<ApiResponse<StatisticsDto>>> GetStatistics(Statistics type)
        {
            var result = await _adminServices.GetStatistics(type);
            return Ok(result);
        }
    }
}
