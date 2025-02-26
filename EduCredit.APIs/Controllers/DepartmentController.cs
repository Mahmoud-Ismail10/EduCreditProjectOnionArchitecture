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

        /// api/Department
        [HttpPost]
        [ProducesResponseType(typeof(Department), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] CreateDepartmentDto createDeptDto)
        {
            if (ModelState.IsValid)
            {
                CreateDepartmentDto? department = await _departmentServices.CreateDepartmentAsync(createDeptDto);
                return Ok(department);
            }
            return BadRequest(new ApiResponse(400));
        }

        /// api/Department
        [HttpGet]
        [ProducesResponseType(typeof(ReadDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public ActionResult<IReadOnlyList<ReadDepartmentDto>> GetDepartments([FromQuery] DepartmentSpecificationParams specParams) // Create class contains all of params (refactor)
        {
            int count;
            var departmentsDto = _departmentServices.GetAllDepartmentAsync(specParams, out count);
            if (departmentsDto is null) return NotFound(new ApiResponse(404));
            return Ok(new Pagination<ReadDepartmentDto>(specParams.PageSize, specParams.PageIndex, count, departmentsDto)); // Status code = 200
        }

        /// api/Department/{id}
        /// Handle not found error in swagger
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReadDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReadDepartmentDto>> GetDepartment(Guid id)
        {
            var departmentDto = await _departmentServices.GetDepartmentByIdAsync(id);
            if (departmentDto is null) return NotFound(new ApiResponse(404)); // Status code = 404
            return Ok(departmentDto); // Status code = 200
        }


    }
}
