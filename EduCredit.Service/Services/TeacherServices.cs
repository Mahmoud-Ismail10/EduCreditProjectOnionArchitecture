using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using System;

namespace EduCredit.Service.Services
{
    public class TeacherServices : ITeacherServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Random _random = new Random();

        public TeacherServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IReadOnlyList<ReadTeacherDto?> GetAllTeachers(TeacherSpecificationParams specParams, out int count)
        {
            var teacherRepo = _unitOfWork.Repository<Teacher>();
            var spec = new TeacherWithDepartmentSpecifications(specParams);
            var teachers = teacherRepo.GetAllSpecification(spec, out count);
            if (teachers is null) return null;
            return _mapper.Map<IReadOnlyList<Teacher>, IReadOnlyList<ReadTeacherDto>>(teachers); ;
        }
        public async Task<ReadTeacherDto?> GetTeacherByIdAsync(Guid id)
        {
            var teacherRepo = _unitOfWork.Repository<Teacher>();
            var spec = new TeacherWithDepartmentSpecifications(id);
            var teacher = await teacherRepo.GetByIdSpecificationAsync(spec);
            if (teacher is null) return null;
            return _mapper.Map<Teacher, ReadTeacherDto>(teacher);
        }

        public async Task<UpdateTeacherDto?> UpdateTeacherAsync(UpdateTeacherDto updateteacherDto, Guid id)
        {
            var spec = new TeacherWithDepartmentSpecifications(id);
            var teacherFromDB = await _unitOfWork.Repository<Teacher>().GetByIdSpecificationAsync(spec);
            if (teacherFromDB is null) return null;

            var newTeacher = _mapper.Map(updateteacherDto, teacherFromDB);
            await _unitOfWork.Repository<Teacher>().Update(newTeacher);

            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return _mapper.Map<Teacher, UpdateTeacherDto>(newTeacher);
        }

        public async Task<ApiResponse> DeleteTeacherAsync(Guid id)
        {
            var teacher = await _unitOfWork.Repository<Teacher>().GetByIdAsync(id);
            if (teacher is null) return new ApiResponse(404);

            await _unitOfWork.Repository<Teacher>().Delete(teacher);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public ReadTeacherDto? AssignGuideToStudent(Guid? departmentId)
        {
            var teachers = _unitOfWork._teacherRepo.GetTeachersAreNotReachMaximumOfStudentsByDepartmentId(departmentId);
            if (teachers is null) return null;

            var selectedTeacher = teachers[_random.Next(teachers.Count)];
            return _mapper.Map<Teacher, ReadTeacherDto>(selectedTeacher);
        }

    }
}
