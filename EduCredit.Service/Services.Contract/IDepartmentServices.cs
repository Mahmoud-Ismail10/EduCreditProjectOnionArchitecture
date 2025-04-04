using EduCredit.Core.Specifications.DepartmentSpecifications;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IDepartmentServices
    {
        Task<CreateDepartmentDto?> CreateDepartmentAsync(CreateDepartmentDto createDeptDto);
        Task<ReadDepartmentDto?> GetDepartmentByIdAsync(Guid id);
        IReadOnlyList<ReadDepartmentDto?> GetAllDepartments(DepartmentSpecificationParams specParams, out int count);
        IReadOnlyList<ReadDepartmentCoursesDto?> GetAllDepartmentsCourses();
        Task<UpdateDepartmentDto?> UpdateDepertmentAsync(UpdateDepartmentDto updateDeptDto, Guid id);
        Task<ApiResponse> DeleteDepartmentAsync(Guid id);
    }
}
