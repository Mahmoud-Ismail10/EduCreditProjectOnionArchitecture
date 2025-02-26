using EduCredit.Core.Specifications.DepartmentSpecifications;
using EduCredit.Service.DTOs.DepartmentDTOs;
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
        void UpdateDepertment(UpdateDepartmentDto updateDeptDto);
        Task<ReadDepartmentDto?> GetDepartmentByIdAsync(Guid id);
        IReadOnlyList<ReadDepartmentDto?> GetAllDepartmentAsync(DepartmentSpecificationParams specParams, out int count);
        void DeleteDepartment(Guid deptId);
    }
}
