using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> DeleteStudnetAsync(Guid id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (student is null) return new ApiResponse(404);

            await _unitOfWork.Repository<Student>().Delete(student);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public IReadOnlyList<ReadStudentDto?> GetAllStudents(StudentSpecificationParams specParams, out int count)
        {
            var spec = new StudentWithDepartmentAndGuideSpecification(specParams);
            var students = _unitOfWork.Repository<Student>().GetAllSpecification(spec, out count);
            if (students is null) return null;

            return _mapper.Map<IReadOnlyList<Student>, IReadOnlyList<ReadStudentDto>>(students);
        }

        public async Task<ReadStudentDto?> GetStudentByIdAsync(Guid id)
        {
            var spec = new StudentWithDepartmentAndGuideSpecification(id);
            var student = await _unitOfWork.Repository<Student>().GetByIdSpecificationAsync(spec);
            if (student is null) return null;

            return _mapper.Map<Student, ReadStudentDto>(student);
        }

        //public async Task<IReadOnlyList<ReadStudentDto>>? GetstudentsByTeacherIdAsync(Guid teacherId)
        //{
        //    var students = await _unitOfWork._studentRepo.GetStudentsByTeacherIdAsync(teacherId);
        //    if (students is null) return null;
        //    return _mapper.Map<IReadOnlyList<Student>, IReadOnlyList<ReadStudentDto>>(students);

        //}

        public async Task<ApiResponse> UpdateStudentAsync(UpdateStudentDto updateStudentDto, Guid id)
        {
            var spec = new StudentWithDepartmentAndGuideSpecification(id);
            var student = await _unitOfWork.Repository<Student>().GetByIdSpecificationAsync(spec);
            if (student is null) return new ApiResponse(404);

            var newStudent = _mapper.Map(updateStudentDto, student);
            await _unitOfWork.Repository<Student>().Update(newStudent);

            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }



    }
}
