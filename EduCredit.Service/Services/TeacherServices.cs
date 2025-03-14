using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.Services.Contract;

namespace EduCredit.Service.Services
{
    public class TeacherServices:ITeacherServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

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
            if (teachers is null) 
                return null;
            var TeachersDto = _mapper.Map<IReadOnlyList<Teacher>, IReadOnlyList<ReadTeacherDto>>(teachers);
            return TeachersDto;
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

        public async Task<bool> DeleteTeacherAsync(Guid id)
        {
            var spec = new TeacherWithDepartmentSpecifications(id);
            var teacher = await _unitOfWork.Repository<Teacher>().GetByIdSpecificationAsync(spec);
            if (teacher is null) return false;

            await _unitOfWork.Repository<Teacher>().Delete(teacher);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
