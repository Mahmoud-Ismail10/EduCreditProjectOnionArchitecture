using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class ScheduleServices : IScheduleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AssignSchedule(CreateScheduleDto createScheduleDto)
        {
            /// Check if course and teacher are exist or no
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(createScheduleDto.CourseId);
            if (course is null) return new ApiResponse(400, "Course not found!");
            var teacher = await _unitOfWork.Repository<Teacher>().GetByIdAsync(createScheduleDto.TeacherId);
            if (teacher is null) return new ApiResponse(400, "Teacher not found!");

            /// Check if Schedule is exist or no
            var existingSchedule = await _unitOfWork._scheduleRepo.GetScheduleByIdsAsync(createScheduleDto.CourseId, createScheduleDto.TeacherId);
            if (existingSchedule is not null) return new ApiResponse(400, "Schedule already exists for this course!");

            /// Mapping data
            var newSchedule = _mapper.Map<CreateScheduleDto, Schedule>(createScheduleDto);

            /// Create Schedule
            await _unitOfWork.Repository<Schedule>().CreateAsync(newSchedule);

            /// Save in DB
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400, "Failed to assign schedule!");
            return new ApiResponse(200, "The schedule was successfully assigned");
        }

        public async Task<ReadScheduleDto?> GetSchedule(Guid courseId, Guid teacherId)
        {
            var schedule = await _unitOfWork._scheduleRepo.GetScheduleByIdsAsync(courseId, teacherId);
            if (schedule is null) return null;
            var scheduleDto = _mapper.Map<Schedule, ReadScheduleDto>(schedule);
            return scheduleDto;
        }

        public async Task<ApiResponse> UpdateSchedule(Guid courseId, Guid teacherId, UpdateScheduleDto updateScheduleDto)
        {
            /// Check if schedule is exist or no
            var schedule = await _unitOfWork._scheduleRepo.GetScheduleByIdsAsync(courseId, teacherId);
            if (schedule is null) return new ApiResponse(404);

            /// Mapping data
            var newSchedule = _mapper.Map(updateScheduleDto, schedule);

            /// Update Schedule
            await _unitOfWork.Repository<Schedule>().Update(newSchedule);

            /// Save in DB
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }
        
        public async Task<ApiResponse> DeleteSchedule(Guid courseId, Guid teacherId)
        {
            /// Check if schedule is exist or no
            var schedule = await _unitOfWork._scheduleRepo.GetScheduleByIdsAsync(courseId, teacherId);
            if (schedule is null) return new ApiResponse(404);

            /// Delete Schedule
            await _unitOfWork.Repository<Schedule>().Delete(schedule);

            /// Save in DB
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }
    }
}
