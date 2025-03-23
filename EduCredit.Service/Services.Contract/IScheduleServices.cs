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
        Task<ReadScheduleDto?> GetSchedule(Guid courseId, Guid teacherId);
        Task<ApiResponse> UpdateSchedule(Guid courseId, Guid teacherId, UpdateScheduleDto updateScheduleDto);
        Task<ApiResponse> DeleteSchedule(Guid courseId, Guid teacherId);
    }
}
