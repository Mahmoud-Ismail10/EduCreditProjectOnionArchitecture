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

namespace EduCredit.Service.Services
{
    public class EnrollmentTableServices : IEnrollmentTableServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISemesterServices _semesterServices;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
      
        //اظهر المواد المتاحه للتسجيل 
        //الشروط
        //1-الطالب خلص السابق للماده
        //2-الطالب لا يسجل الماده من قبل
        //4-الماده متاحه للتسجيل في هذا الترم 
        //5-أن يكون الطالب قد رسب فيها من قب لإن كان قد سجلها سابقًا ولم ينجح 
        public EnrollmentTableServices(IUnitOfWork unitOfWork, ISemesterServices semesterServices, IMapper mapper, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _semesterServices = semesterServices;
            _mapper = mapper;
            _cache = cache;
            _mapper = mapper;
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
                    return new ApiResponse(200, "Enrollment Table updated successfully");
                }
            }
            return new ApiResponse(400, "Failed to create enrollment table, Because the enrollment period ended!");
        }

        //public Task<IReadOnlyList<ReadEnrollmentTableDto>?> GetStudentAvailableCourses(string studentId)
        //{
        //    throw new NotImplementedException();
        //}

        //        //3-الطالب لا يسجل الماده في نفس الوقت
        //        //5-الطالب لا يسجل اكثر من 18 ساعه في الترم الحالي
        //        //6-الطالب لا يسجل اكثر من 10 ساعات في الصيفي

    }
}