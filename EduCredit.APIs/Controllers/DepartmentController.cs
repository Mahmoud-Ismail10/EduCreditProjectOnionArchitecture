using AutoMapper;
using EduCredit.Service.Errors;
using EduCredit.Service.DTOs.DepartmentDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using EduCredit.Service.Services;
using EduCredit.Core.Security;
using Microsoft.AspNetCore.Authorization;
namespace EduCredit.APIs.Controllers
{
    public class DepartmentController : BaseApiController
    {
        private readonly IDepartmentServices _departmentServices;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentServices departmentServices, IMapper mapper)
        {
            _departmentServices = departmentServices;
            _mapper = mapper;
        }

        /// POST: api/Department
        [HttpPost]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse< CreateDepartmentDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<CreateDepartmentDto>>> CreateDepartment([FromBody] CreateDepartmentDto createDeptDto)
        {
            if (ModelState.IsValid)
            {
                var departmentDto = await _departmentServices.CreateDepartmentAsync(createDeptDto);
                if(departmentDto is not null)
                    return Ok(new ApiResponse<CreateDepartmentDto>(201,"Department Created Successfully",departmentDto));
                return BadRequest(new ApiResponse(400,"This department is already exists !"));
            }
            return BadRequest(new ApiResponse(400));
        }

        /// GET: api/Department
        [HttpGet]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(Pagination<ReadDepartmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<IReadOnlyList<ReadDepartmentDto>> GetDepartments([FromQuery] DepartmentSpecificationParams specParams) // Create class contains all of params (refactor)
        {
            int count;
            var departmentsDto = _departmentServices.GetAllDepartments(specParams, out count);
            if (departmentsDto is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadDepartmentDto>(specParams.PageSize, specParams.PageIndex, count, departmentsDto)); // Status code = 200
        }

        [HttpGet("departments-with-courses")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ReadDepartmentCoursesDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<ApiResponse<IReadOnlyList<ReadDepartmentCoursesDto>>> Getdepartmentswithcourses() // Create class contains all of params (refactor)
        {
            var departmentsDto = _departmentServices.GetAllDepartmentsCourses();
            if (departmentsDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<IReadOnlyList<ReadDepartmentCoursesDto>>(200, "Success", departmentsDto));

            // Status code = 200
        }

        /// GET: api/Department/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse<ReadDepartmentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponse<ReadDepartmentDto>>> GetDepartment(Guid id)
        {
            var departmentDto = await _departmentServices.GetDepartmentByIdAsync(id);
            if (departmentDto is null) return NotFound(new ApiResponse(404));
            return Ok(new ApiResponse<ReadDepartmentDto>(200,"success",departmentDto));
        }

        /// PUT: api/Department/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentDto updateDeptDto)
        {
            if (ModelState.IsValid)
            {
                var departmentDto = await _departmentServices.UpdateDepertmentAsync(updateDeptDto, id);
                if (departmentDto is null) return NotFound(new ApiResponse(404));
                return Ok(new ApiResponse(200,"Updated Successfully"));
            }
            return BadRequest(new ApiResponse(400));
        }

        /// DELETE: api/Department/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteDepartment(Guid id)
        {
            var response = await _departmentServices.DeleteDepartmentAsync(id);
           
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Department deleted successfully"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Department not found!"));
            else
                return BadRequest(new ApiResponse(400, response.Message));
        }
    }
}
