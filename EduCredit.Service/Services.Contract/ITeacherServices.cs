using EduCredit.Core.Enums;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ITeacherServices
    {
        IReadOnlyList<ReadTeacherDto?> GetAllTeachers(TeacherSpecificationParams specParams, out int count);
        Task<ReadTeacherDto?> GetTeacherByIdAsync(Guid id);
        Task<UpdateTeacherDto?> UpdateTeacherAsync(UpdateTeacherDto updateteacherDto, Guid id);
        Task<ApiResponse> DeleteTeacherAsync(Guid id);
        ReadTeacherDto? AssignGuideToStudent(Guid? departmentId);
        Task<StatisticsDto> GetStatistics(TeacherStatistics type, Guid TeacherId);
    }
}
