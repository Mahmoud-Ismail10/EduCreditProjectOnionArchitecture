using AutoMapper;
using EduCredit.Core.Models;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.DTOs.DepartmentDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduCredit.Service.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, ReadDepartmentDto>();

            CreateMap<CreateDepartmentDto, Department>();

            CreateMap<RegisterAdminDto, Person>().ReverseMap();
            CreateMap<RegisterStudentAndTeacherDto, Person>().ReverseMap();
        }
    }
}
