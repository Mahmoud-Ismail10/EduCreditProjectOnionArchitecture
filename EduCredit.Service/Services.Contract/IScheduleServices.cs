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
       // Task<ApiResponse> AssignSchedule(CreateScheduleDto createScheduleDto);
        Task<ReadScheduleDto?> GetSchedule(Guid ScheduleId);
        Task<ApiResponse> UpdateSchedule(Guid courseId, UpdateScheduleDto updateScheduleDto);
        Task<ApiResponse> DeleteSchedule(Guid courseId);
        Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetStudentAvailableCourses(Guid studentId);
        Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetSchedulesByStudentId(Guid stuId);
    }
}
