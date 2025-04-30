using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.AdminDTOs;
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
            if (teacher.Students.Count > 0)
            {
                return new ApiResponse(400, "Students are associated with this Teacher. Please delete the students first.");
            }
            await _unitOfWork.Repository<Teacher>().Delete(teacher);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public ReadTeacherDto? AssignGuideToStudent(Guid? departmentId)
        {
            var teachers = _unitOfWork._teacherRepo.GetTeachersAreNotReachMaximumOfStudentsByDepartmentId(departmentId);
            if (teachers == null || !teachers.Any())
                return null;

            var selectedTeacher = teachers[_random.Next(teachers.Count)];
            //var selectedTeacher = teachers[_random.Next(teachers.Count)];
            return _mapper.Map<Teacher, ReadTeacherDto>(selectedTeacher);
        }

        public async Task<StatisticsDto> GetStatistics(TeacherStatistics statistics,Guid? TeacherId)
        { 

            var total = statistics switch
            {
                TeacherStatistics.Courses => await _unitOfWork.Repository<Course>().CountAsync(new CourseWithDeptAndPrevCourseSpecification(TeacherId)),
                TeacherStatistics.Students => await _unitOfWork.Repository<Student>().CountAsync(new StudentWithDepartmentAndGuideSpecification(TeacherId)),
                //Statistics.SuccessRate => await GetSuccessRateAsync(),
                _=>0
            };

            if (!Enum.IsDefined(typeof(TeacherStatistics), statistics))
            {
                return new StatisticsDto
                {
                    Type = "Invalid",
                    Total = 0
                };
            }
            return new StatisticsDto
            {
                Type = statistics.ToString(),
                Total = total
            };
        }
        //private async Task<double> GetSuccessRateAsync()
        //{
            

        //    var enrolledSpec = new EnrollmentsWithCoursesSpecification(passedOnly: true);

        //    int totalEnrolled = await _unitOfWork
        //        .Repository<Enrollment>()
        //        .CountAsync(enrolledSpec);

        //    if (totalEnrolled == 0)
        //        return 0;

        //    return Math.Round((double)totalPassed / totalEnrolled * 100, 2);
        //}


    }
}
