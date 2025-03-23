using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ICourseServices
    {
        Task<ApiResponse> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task<ReadCourseDto?> GetCourseByIdAsync(Guid id);
        IReadOnlyList<ReadCourseDto?> GetAllCourses(CourseSpecificationParams specParams, out int count);
        Task<ApiResponse> UpdateCourseAsync(UpdateCourseDto updateCourseDto, Guid id);
        Task<ApiResponse> DeleteCourseAsync(Guid id);
    }
}
