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

        public IReadOnlyList<ReadDepartmentDto?> GetAllDepartments(DepartmentSpecificationParams specParams, out int count)
        {
            var spec = new DepartmentWithTeacherSpecifications(specParams);
            var departments = _unitOfWork.Repository<Department>().GetAllSpecification(spec, out count);
            if (departments is null) return null;
            return _mapper.Map<IReadOnlyList<Department>, IReadOnlyList<ReadDepartmentDto>>(departments);
        }

        public async Task<ReadDepartmentDto?> GetDepartmentByIdAsync(Guid id)
        {
            var spec = new DepartmentWithTeacherSpecifications(id);
            var department = await _unitOfWork.Repository<Department>().GetByIdSpecificationAsync(spec);
            if (department is null) return null;
            return _mapper.Map<Department, ReadDepartmentDto>(department);
        }

        public async Task<UpdateDepartmentDto?> UpdateDepertmentAsync(UpdateDepartmentDto updateDeptDto, Guid id)
        {
            var spec = new DepartmentWithTeacherSpecifications(id);
            var departmentFromDB = await _unitOfWork.Repository<Department>().GetByIdSpecificationAsync(spec);
            if (departmentFromDB is null) return null;

            var newDepartment = _mapper.Map(updateDeptDto, departmentFromDB);
            await _unitOfWork.Repository<Department>().Update(newDepartment);

            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return updateDeptDto;
        }

        public async Task<ApiResponse> DeleteDepartmentAsync(Guid id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return new ApiResponse(404);

            await _unitOfWork.Repository<Department>().Delete(department);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }
    }
}
