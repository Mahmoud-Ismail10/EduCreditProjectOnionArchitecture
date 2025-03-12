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
        [ProducesResponseType(typeof(CreateDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ReadDepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto createDeptDto)
        {
            if (ModelState.IsValid)
            {
                var departmentDto = await _departmentServices.CreateDepartmentAsync(createDeptDto);
                return Ok(departmentDto);
            }
            return BadRequest(new ApiResponse(400));
        }

        /// GET: api/Department
        [HttpGet]
        [Cache(30)]
        [ProducesResponseType(typeof(ReadDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<IReadOnlyList<ReadDepartmentDto>> GetDepartments([FromQuery] DepartmentSpecificationParams specParams) // Create class contains all of params (refactor)
        {
            int count;
            var departmentsDto = _departmentServices.GetAllDepartments(specParams, out count);
            if (departmentsDto is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadDepartmentDto>(specParams.PageSize, specParams.PageIndex, count, departmentsDto)); // Status code = 200
        }

        /// GET: api/Department/{id}
        [HttpGet("{id}")]
        [Cache(30)]
        [ProducesResponseType(typeof(ReadDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReadDepartmentDto>> GetDepartment(Guid id)
        {
            var departmentDto = await _departmentServices.GetDepartmentByIdAsync(id);
            if (departmentDto is null) return NotFound(new ApiResponse(404));
            return Ok(departmentDto);
        }

        /// PUT: api/Department/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UpdateDepartmentDto>> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentDto updateDeptDto)
        {
            if (ModelState.IsValid)
            {
                var departmentDto = await _departmentServices.UpdateDepertmentAsync(updateDeptDto, id);
                if (departmentDto is null) return NotFound(new ApiResponse(404));
                return Ok(departmentDto);
            }
            return BadRequest(new ApiResponse(400));
        }

        /// DELETE: api/Department/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteDepartment(Guid id)
        {
            var response = await _departmentServices.DeleteDepartmentAsync(id);
            if (response.StatusCode == 200)
                return Ok(new ApiResponse(200, "Department deleted successfully"));
            else if (response.StatusCode == 404)
                return NotFound(new ApiResponse(404, "Department not found!"));
            else
                return BadRequest(new ApiResponse(400, "It is not suitable to delete the Department!"));
        }
    }
}
