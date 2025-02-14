using AutoMapper;
using EduCredit.APIs.DTOs.DepartmentDTOs;
using EduCredit.APIs.Errors;
using EduCredit.APIs.Helper;
using EduCredit.Core.Models;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using EduCredit.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EduCredit.APIs.Controllers
{
    public class DepartmentController : BaseApiController
    {
        private readonly IGenericRepository<Department> _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentController(IGenericRepository<Department> departmentRepo, IMapper mapper)
        {
            _departmentRepo = departmentRepo;
            _mapper = mapper;
        }

        /// api/Department
        //[HttpPost]
        //public async Task<ActionResult<Department>> CreateDepartment(CreateDepartmentDTO createDepartmentDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var department = new Department
        //    {
        //        Name = createDepartmentDTO.Name,
        //        DepartmentHeadId = createDepartmentDTO.DepartmentHeadId
        //    };

        //    _dbcontext.Departments.Add(department);
        //    await _dbcontext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        //}

        /// api/Department
        [HttpGet("GetDepartments")]
        public ActionResult<IReadOnlyList<ReadDepartmentDto>> GetDepartments([FromQuery] DepartmentSpecificationParams specParams) // Create class contains all of params (refactor)
        {
            int count;
            var spec = new DepartmentWithTeacherSpecifications(specParams);
            var departments = _departmentRepo.GetAllSpecification(spec, out count);
            if (departments == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var data = _mapper.Map<IReadOnlyList<Department>, IReadOnlyList<ReadDepartmentDto>>(departments);
            return Ok(new Pagination<ReadDepartmentDto>(specParams.PageSize, specParams.PageIndex, count, data)); // Status code = 200
        }

        /// api/Department/1
        /// Handle not found error in swagger
        [HttpGet("GetDepartment/{id}")]
        [ProducesResponseType(typeof(ReadDepartmentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReadDepartmentDto>> GetDepartment(Guid id)
        {
            var spec = new DepartmentWithTeacherSpecifications(id);
            var department = await _departmentRepo.GetByIdSpecificationAsync(spec);
            if (department == null)
            {
                return NotFound(new ApiResponse(404)); // Status code = 404
            }
            return Ok(_mapper.Map<Department, ReadDepartmentDto>(department)); // Status code = 200
        }


    }
}
