using EduCredit.Service.DTOs.SemesterCourseDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ISemesterServices
    {
        Task<ApiResponse> CreateSemester(CreateSemesterDto createSemesterDto);
        Task<ApiResponse> UpdateSemester(UpdateSemesterDto updateSemesterDto, Guid semesterId);
        Task<ApiResponse> DeleteSemester(Guid semesterId);
        Task<ApiResponse> AssignCoursesToSemester(SemesterCourseDto semesterCourseDto);
    }
}
