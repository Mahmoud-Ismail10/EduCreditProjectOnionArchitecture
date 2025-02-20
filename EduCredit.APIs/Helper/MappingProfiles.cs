using AutoMapper;
using EduCredit.APIs.DTOs.AuthDTOs;
using EduCredit.APIs.DTOs.DepartmentDTOs;
using EduCredit.Core.Models;

namespace EduCredit.APIs.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, ReadDepartmentDto>()
                .ForMember(d => d.DepartmentHeadName, o => o.MapFrom(s => s.DepartmentHead.FullName));
        }
    }
}
