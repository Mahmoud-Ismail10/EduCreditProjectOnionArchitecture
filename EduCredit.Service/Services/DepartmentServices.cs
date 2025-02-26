using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Repositories.Contract;
using EduCredit.Core.Services.Contract;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateDepartmentDto?> CreateDepartmentAsync(CreateDepartmentDto createDeptDto)
        {
            /// Map the incoming DTO to the Department entity
            Department department = _mapper.Map<CreateDepartmentDto, Department>(createDeptDto);

            /// Create the department in the database
            await _unitOfWork.Repository<Department>().CreateAsync(department);

            /// Save changes to the database
            int result = await _unitOfWork.CompleteAsync();

            /// If no changes were saved, return null else return Dto
            if (result <= 0) return null;
            return createDeptDto;
        }

        public void DeleteDepartment(Guid deptId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ReadDepartmentDto?> GetAllDepartmentAsync(DepartmentSpecificationParams specParams, out int count)
        {
            var departmentRepo = _unitOfWork.Repository<Department>();
            var spec = new DepartmentWithTeacherSpecifications(specParams);
            var departments = departmentRepo.GetAllSpecification(spec, out count);
            if (departments is null) return null;
            var departmentsDto = _mapper.Map<IReadOnlyList<Department>, IReadOnlyList<ReadDepartmentDto>>(departments);
            return departmentsDto;
        }

        public async Task<ReadDepartmentDto?> GetDepartmentByIdAsync(Guid id)
        {
            var departmentRepo = _unitOfWork.Repository<Department>();
            var spec = new DepartmentWithTeacherSpecifications(id);
            var department = await departmentRepo.GetByIdSpecificationAsync(spec);
            if (department is null) return null;
            var departmentDto = _mapper.Map<Department, ReadDepartmentDto>(department);
            return departmentDto;
        }

        public void UpdateDepertment(UpdateDepartmentDto updateDeptDto)
        {
            throw new NotImplementedException();
        }
    }
}
