using AutoMapper;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.DTOs.TeacherDTOs;
namespace EduCredit.Service.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, ReadDepartmentDto>()
                .ForMember(d => d.DepartmentHeadFullName, o => o.MapFrom(s => s.DepartmentHead.FullName)).ReverseMap();
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
            CreateMap<UpdateDepartmentDto, Department>().ReverseMap();

            CreateMap<Course, ReadCourseDto>()
                .ForMember(d => d.PreviousCourseName, o => o.MapFrom(s => s.PreviousCourse.Name))
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name)).ReverseMap();
            CreateMap<CreateCourseDto, Course>().ReverseMap();
            CreateMap<UpdateCourseDto, Course>().ReverseMap();
            CreateMap <Course,ReadCourseDto>().ReverseMap();
           
            CreateMap<CreateSemesterDto, Semester>();
            CreateMap<UpdateSemesterDto, Semester>();

            //CreateMap<EnrollmentDto, Enrollment>();


            CreateMap<Teacher, ReadTeacherDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
               .ForMember(d => d.DepartmentFullName, o => o.MapFrom(s => s.Department.Name))
               .ReverseMap();
            CreateMap<UpdateTeacherDto, Teacher>().ReverseMap();

            CreateMap<Student, ReadStudentDto>();
            CreateMap<Student, UpdateStudentDto>();

            CreateMap<BaseRegisterDto, Person>().ReverseMap();
            CreateMap<Admin, ReadAdminDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
              .ReverseMap();
            CreateMap<UpdateAdminDto, Admin>().ReverseMap();

            CreateMap<BaseUserDto, Person>().ReverseMap();

            CreateMap< Person, BaseRegisterDto>()
                .ForMember(d=>d.FullName, o=>o.MapFrom(s=>s.FullName)).ReverseMap();
        }
    }
}
