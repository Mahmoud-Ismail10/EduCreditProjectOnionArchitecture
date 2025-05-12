using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.Errors;
using EduCredit.Service.DTOs.CourseDTOs;
using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.Services.Contract;
using EduCredit.Core.Specifications.EnrollmentTableSpecifications;
using Microsoft.AspNetCore.SignalR;
using EduCredit.Service.Hubs;

namespace EduCredit.Service.Services
{
    public class EnrollmentTableServices : IEnrollmentTableServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISemesterServices _semesterServices;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly INotificationServices _Notification;
        public EnrollmentTableServices(IUnitOfWork unitOfWork, ISemesterServices semesterServices, IMapper mapper, ICacheService cache, INotificationServices Notification)
        {
            _unitOfWork = unitOfWork;
            _semesterServices = semesterServices;
            _mapper = mapper;
            _cache = cache;
            _mapper = mapper;
            _Notification = Notification;
        }

        public async Task<ApiResponse> CreateOrUpdateEnrollmentTable(CreateOrUpdateEnrollmentTableDto createOrUpdateEnrollmentTableDto)
        {
            if (await _semesterServices.IsEnrollmentOpenAsync())
            {
                createOrUpdateEnrollmentTableDto.Status = Status.Pending;

                var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
                if (currentSemester is null) return new ApiResponse(404, "There is no current semester!");
                createOrUpdateEnrollmentTableDto.SemesterId = currentSemester.Id;

                var spec = new EnrollmentTableWithSemesterAndStudentSpecification((Guid)createOrUpdateEnrollmentTableDto.StudentId, currentSemester.Id);
                var existEnrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdSpecificationAsync(spec);
                /// When the student enrolls the table for the first time in the current semester 
                if (existEnrollmentTable is null)
                {
                    var createEnrollmentTable = _mapper.Map<CreateOrUpdateEnrollmentTableDto, EnrollmentTable>(createOrUpdateEnrollmentTableDto);

                    // Fetch all valid course IDs from DB
                    var existingScheduleIds = await _unitOfWork._scheduleRepo.GetValidScheduleIds(createOrUpdateEnrollmentTableDto.ScheduleIds);
                    // Check if all requested courses exist
                    var missingSchedules = createOrUpdateEnrollmentTableDto.ScheduleIds.Except(existingScheduleIds).ToList();
                    if (missingSchedules.Any())
                        return new ApiResponse(400, $"Schedules not found: {string.Join(", ", missingSchedules)}");

                    createEnrollmentTable.Enrollments = createOrUpdateEnrollmentTableDto.ScheduleIds
                        .Select(scheduleId => new Enrollment { EnrollmentTableId = createEnrollmentTable.Id, CourseId = scheduleId })
                        .ToList();

                    await _unitOfWork.Repository<EnrollmentTable>().CreateAsync(createEnrollmentTable);
                    int result = await _unitOfWork.CompleteAsync();
                    if (result <= 0) return new ApiResponse(400, "Failed to create enrollment table!");
                    // Notify the teacher about the new enrollment
                    if (createEnrollmentTable.Student.TeacherId != null)
                    {
                        await _Notification.SendNotificationToTeacherAsync(createEnrollmentTable.Student.FullName, createEnrollmentTable.Student.TeacherId);
                    }

                    return new ApiResponse(200, "Enrollment Table created successfully");

                }
                /// When the student has already enrolled the table and tries to update it
                else
                {
                    var updateEnrollmentTable = _mapper.Map(createOrUpdateEnrollmentTableDto, existEnrollmentTable);

                    // Fetch current enrollments in this enrollment table
                    var currentEnrollments = existEnrollmentTable.Enrollments.Select(e => e.CourseId).ToList();
                    // Fetch the enrollments that must be deleted from enrollment table if exist
                    var enrollmentsToRemove = existEnrollmentTable.Enrollments
                        .Where(enrollment => !createOrUpdateEnrollmentTableDto.ScheduleIds.Contains(enrollment.CourseId))
                        .ToList();
                    if (enrollmentsToRemove.Any())
                        await _unitOfWork.Repository<Enrollment>().DeleteRange(enrollmentsToRemove);
                    // Add new enrollments to enrollment table if exist
                    var newEnrollments = createOrUpdateEnrollmentTableDto.ScheduleIds
                        .Where(scheduleId => !currentEnrollments.Contains(scheduleId))
                        .Select(scheduleId => new Enrollment { EnrollmentTableId = updateEnrollmentTable.Id, CourseId = scheduleId })
                        .ToList();
                    if (newEnrollments.Any())
                        await _unitOfWork.Repository<Enrollment>().CreateRangeAsync(newEnrollments);

                    await _unitOfWork.Repository<EnrollmentTable>().Update(updateEnrollmentTable);
                    int result = await _unitOfWork.CompleteAsync();
                    if (result <= 0) return new ApiResponse(400, "Failed to update enrollment table!");
                    if (updateEnrollmentTable.Student.TeacherId != null)
                    {
                      await  _Notification.SendNotificationToTeacherAsync(updateEnrollmentTable.Student.FullName, updateEnrollmentTable.Student.TeacherId);
                    }
                    return new ApiResponse(200, "Enrollment Table updated successfully");
                }
            }
            return new ApiResponse(400, "Failed to create enrollment table, Because the enrollment period ended!");
        }

        public async Task<CreateOrUpdateEnrollmentTableDto> GetEnrollmentTableByStudentId(Guid studentId)
        {
            var spec = new EnrollmentTableWithSemesterAndStudentSpecification(studentId);
            var enrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdSpecificationAsync(spec);
            if (enrollmentTable is null) return null;
            return _mapper.Map<EnrollmentTable, CreateOrUpdateEnrollmentTableDto>(enrollmentTable);
        }

        public async Task<ApiResponse> UpdateEnrollmentTableStatus(Guid EnrollmentTableId, UpdateEnrollmentTableDto dto)
        {
            var enrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdAsync(EnrollmentTableId);
            if (enrollmentTable is null) return new ApiResponse(404, "This enrollment table does not exist!");

            if (enrollmentTable.Status == dto.Status.Value)
                return new ApiResponse(400, $"This enrollment table is already {dto.Status.Value}!");

            enrollmentTable.Status = dto.Status.Value;

            if (!string.IsNullOrWhiteSpace(dto.GuideNotes))
                enrollmentTable.GuideNotes = dto.GuideNotes;

            await _unitOfWork.Repository<EnrollmentTable>().Update(enrollmentTable);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400, "Failed to update enrollment table status!");

            return new ApiResponse(200, $"Enrollment Table status updated to {dto.Status.Value} successfully");
        }
    }
}