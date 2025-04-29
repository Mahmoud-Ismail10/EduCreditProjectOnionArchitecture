using AutoMapper;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.DTOs.ScheduleDTOs;
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
            CreateMap<Department, ReadDepartmentCoursesDto>()
                .ForMember(d => d.Courses, o => o.MapFrom(s => s.Courses))
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Name))
                .ReverseMap();
            CreateMap<Course, ReadCourseDto>()
                .ForMember(d => d.PreviousCourseName, o => o.MapFrom(s => s.PreviousCourse.Name))
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name)).ReverseMap();
            CreateMap<Course, departmentcourseDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ReverseMap();

            CreateMap<CreateCourseDto, Course>().ReverseMap();

            CreateMap<UpdateCourseDto, Course>().ReverseMap();
            CreateMap<Course, ReadCourseDto>().ReverseMap();

            CreateMap<CreateSemesterDto, Semester>()
                .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.SemesterType}-{s.Year}"));
            CreateMap<UpdateSemesterDto, Semester>()
                .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.SemesterType}-{s.Year}"));
            CreateMap<Semester, ReadSemesterDto>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules))
                .ForMember(d => d.Year, o => o.MapFrom(s => s.EndDate.Year))
                .ForMember(d => d.SemesterType, o => o.Ignore())
                .ReverseMap();

            CreateMap<CreateOrUpdateEnrollmentTableDto, EnrollmentTable>();

            //CreateMap<Schedule, ReadScheduleDto>()
            //    .ForMember(d => d.TeachersName, o => o.MapFrom(s => s.TeacherSchedules
            //    .Select(ts => ts.Teacher != null ? ts.Teacher.FullName : string.Empty)
            //    .ToList()));
            CreateMap<CreateScheduleDto, Schedule>();
            CreateMap<UpdateScheduleDto, Schedule>();

            CreateMap<Enrollment, ReadEnrollmentDto>();
            CreateMap<CreateEnrollmentDto, Enrollment>();
            CreateMap<UpdateEnrollmentDto, Enrollment>();

            CreateMap<Schedule, ReadScheduleDto>()
                .ForMember(d => d.TeachersName, o => o.MapFrom(s =>
                    string.Join(", ", s.TeacherSchedules.Select(ts => ts.Teacher != null ? ts.Teacher.FullName : string.Empty))))
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.Name))
                .ForMember(d => d.Duration, o => o.MapFrom(s => s.Course.Duration))
                .ForMember(d => d.Hours, o => o.MapFrom(s => s.Course.CreditHours));


            //CreateMap<Teacher, TeacherDropDownListDto>()
            //    .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName));


            CreateMap<Teacher, ReadTeacherDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
               .ForMember(d => d.DepartmentFullName, o => o.MapFrom(s => s.Department.Name))
               .ReverseMap();
            CreateMap<UpdateTeacherDto, Teacher>().ReverseMap();

            CreateMap<Student, ReadStudentDto>()
                .ForMember(d => d.AcademicGuide, o => o.MapFrom(s => s.Teacher.FullName));
            CreateMap<UpdateStudentDto, Student>()
                .ReverseMap();

            CreateMap<BaseRegisterDto, Person>().ReverseMap();
            CreateMap<Admin, ReadAdminDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
              .ReverseMap();
            CreateMap<UpdateAdminDto, Admin>().ReverseMap();

            CreateMap<BaseUserDto, Person>().ReverseMap();

            CreateMap<Person, BaseRegisterDto>()
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName)).ReverseMap();
        }
    }
}
