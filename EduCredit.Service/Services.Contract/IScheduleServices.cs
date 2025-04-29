using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IScheduleServices
    {
        Task<ApiResponse> AssignSchedule(CreateScheduleDto createScheduleDto);
        Task<ReadScheduleDto?> GetSchedule(Guid CourseId, Guid SemesterId);
        Task<ApiResponse> UpdateSchedule(Guid CourseId, Guid SemesterId, UpdateScheduleDto updateScheduleDto);
        Task<ApiResponse> DeleteSchedule(Guid CourseId, Guid SemesterId);
        Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetStudentWithAvailableCourses(Guid studentId);
        Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetSchedulesByStudentId(Guid stuId);
    }
}
