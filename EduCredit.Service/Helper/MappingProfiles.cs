using AutoMapper;
using EduCredit.Core.Models;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.UserDTOs;

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

            CreateMap<RegisterAdminDto, Person>().ReverseMap();
            CreateMap<RegisterStudentAndTeacherDto, Person>().ReverseMap();

            CreateMap< Person, GetUserInfoDto>()
                .ForMember(d=>d.Name,o=>o.MapFrom(s=>s.FullName)).ReverseMap();
        }
    }
}
