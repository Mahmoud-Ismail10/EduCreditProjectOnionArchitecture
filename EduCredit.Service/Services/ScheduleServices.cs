using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.CourseSpecifications;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Core.Specifications.EnrollmentTableSpecifications;
using EduCredit.Core.Specifications.ScheduleSpecifications;
//using EduCredit.Core.Specifications.SemesterCoursesSpecifications;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Core.Specifications.TeacherSchedualeSpecifications;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;

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
            /// Get the current semester
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return new ApiResponse(400, "There is no current semester!");

            /// Check if Schedule is exist or no
            var existingSchedule = await _unitOfWork._scheduleRepo.GetScheduleByCourseIdAndSemesterIdAsync(createScheduleDto.CourseId, semester.Id);
            if (existingSchedule is not null) return new ApiResponse(400, "Schedule already exists in current semester!");

            /// Check if course is exist or no
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(createScheduleDto.CourseId);
            if (course is null) return new ApiResponse(400, "Course not found!");

            /// Check if all requested teachers exist or no
            var existingTeacherIds = await _unitOfWork._teacherRepo.GetValidTeacherIds(createScheduleDto.TeacherIds);
            var missingTeachers = createScheduleDto.TeacherIds.Except(existingTeacherIds).ToList();
            if (missingTeachers.Any())
                return new ApiResponse(400, $"Teachers not found: {string.Join(", ", missingTeachers)}");

            /// Mapping data
            createScheduleDto.SemesterId = semester.Id;
            var newSchedule = _mapper.Map<CreateScheduleDto, Schedule>(createScheduleDto);

            newSchedule.TeacherSchedules = createScheduleDto.TeacherIds
                    .Select(teacherId => new TeacherSchedule { CourseId = newSchedule.CourseId, SemesterId = semester.Id, TeacherId = teacherId })
                    .ToList();

            /// Create Schedule
            await _unitOfWork.Repository<Schedule>().CreateAsync(newSchedule);

            /// Save in DB
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400, "Failed to assign schedule!");
            return new ApiResponse(200, "The schedule was successfully assigned");
        }

        public async Task<ReadScheduleDto?> GetSchedule(Guid CourseId, Guid SemesterId)
        {
            var scheduleSpec = ScheduleSpecification.ByCourseAndSemester(CourseId, SemesterId);
            var scheduleSpecList = await _unitOfWork.Repository<Schedule>().GetByIdSpecificationAsync(scheduleSpec);
            if (scheduleSpecList is null) return null;
            var scheduleMapped = new ReadScheduleDto
            {
                CourseId = scheduleSpecList.CourseId,
                SemesterName = scheduleSpecList.Semester.Name,
                CourseName = scheduleSpecList.Course.Name,
                CreditHours = scheduleSpecList.Course.CreditHours,
                MinimumDegree = scheduleSpecList.Course.MinimumDegree,
                PreviousCourse = scheduleSpecList.Course.PreviousCourse?.Name,
                TeachersName = string.Join(", ", scheduleSpecList.TeacherSchedules?
                                            .Where(s => s.Teacher != null)
                                            .Select(t => t.Teacher.FullName)),

                Duration = scheduleSpecList.Course.Duration,
                Hours = scheduleSpecList.Course.CreditHours,
                Day = scheduleSpecList.Day,
                ExamDate = scheduleSpecList.ExamDate,
                ExamEnd = scheduleSpecList.ExamEnd,
                ExamLocation = scheduleSpecList.ExamLocation,
                ExamStart = scheduleSpecList.ExamStart,
                LectureEnd = scheduleSpecList.LectureEnd,
                LectureLocation = scheduleSpecList.LectureLocation,
                LectureStart = scheduleSpecList.LectureStart,
            };
            return scheduleMapped;
        }
        
        public async Task<ReadScheduleDto?> GetSchedule(Guid CourseId)
        {
            /// Check if current semester is exist or no
            var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (currentSemester is null) return null;
            var scheduleSpec = ScheduleSpecification.ByCourseAndSemester(CourseId, currentSemester.Id);
            var scheduleSpecList = await _unitOfWork.Repository<Schedule>().GetByIdSpecificationAsync(scheduleSpec);
            if (scheduleSpecList is null) return null;
            var scheduleMapped = new ReadScheduleDto
            {
                CourseId = scheduleSpecList.CourseId,
                SemesterName = scheduleSpecList.Semester.Name,
                CourseName = scheduleSpecList.Course.Name,
                CreditHours = scheduleSpecList.Course.CreditHours,
                MinimumDegree = scheduleSpecList.Course.MinimumDegree,
                PreviousCourse = scheduleSpecList.Course.PreviousCourse?.Name,
                TeachersName = string.Join(", ", scheduleSpecList.TeacherSchedules?
                                            .Where(s => s.Teacher != null)
                                            .Select(t => t.Teacher.FullName)),

                Duration = scheduleSpecList.Course.Duration,
                Hours = scheduleSpecList.Course.CreditHours,
                Day = scheduleSpecList.Day,
                ExamDate = scheduleSpecList.ExamDate,
                ExamEnd = scheduleSpecList.ExamEnd,
                ExamLocation = scheduleSpecList.ExamLocation,
                ExamStart = scheduleSpecList.ExamStart,
                LectureEnd = scheduleSpecList.LectureEnd,
                LectureLocation = scheduleSpecList.LectureLocation,
                LectureStart = scheduleSpecList.LectureStart,
            };
            return scheduleMapped;
        }

        public async Task<ApiResponse> UpdateSchedule(Guid CourseId, UpdateScheduleDto updateScheduleDto)
        {
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return new ApiResponse(404, "There is no current semester!");

            var spec = ScheduleSpecification.ByCourseAndSemester(CourseId, semester.Id);
            var schedule = await _unitOfWork.Repository<Schedule>().GetByIdSpecificationAsync(spec);
            if (schedule is null) return new ApiResponse(404);

            // 1. Remove old TeacherSchedules
            if (schedule.TeacherSchedules != null && schedule.TeacherSchedules.Any())
            {
                await _unitOfWork.Repository<TeacherSchedule>().DeleteRange(schedule.TeacherSchedules.ToList());
            }

            // 2. Update basic schedule info
            _mapper.Map(updateScheduleDto, schedule);

            // 3. Add new TeacherSchedules
            schedule.TeacherSchedules = updateScheduleDto.TeacherIds
                .Distinct()
                .Select(teacherId => new TeacherSchedule
                {
                    TeacherId = teacherId,
                    CourseId = CourseId,
                    SemesterId = semester.Id
                })
                .ToList();

            // 4. Save updated schedule
            await _unitOfWork.Repository<Schedule>().Update(schedule);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return new ApiResponse(400, "Failed to update schedule!");
            return new ApiResponse(200);
        }

        public async Task<ApiResponse> DeleteSchedule(Guid CourseId)
        {
            /// Check if current semester is exist or no
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return new ApiResponse(404, "There is no current semester!");

            /// Check if schedule is exist or no
            var spec = ScheduleSpecification.ByCourseAndSemester(CourseId, semester.Id);
            var scheduleSpecList = await _unitOfWork.Repository<Schedule>().GetByIdSpecificationAsync(spec);
            if (scheduleSpecList is null) return new ApiResponse(404);

            /// Delete Schedule
            await _unitOfWork.Repository<Schedule>().Delete(scheduleSpecList);

            /// Save in DB
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public async Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetStudentWithAvailableCourses(Guid stuId)
        {
            // Retrieve student data
            var studentSpec = new StudentWithDepartmentAndGuideSpecification(stuId);
            var student = await _unitOfWork.Repository<Student>().GetByIdSpecificationAsync(studentSpec);
            if (student is null) return null;

            // Get the current semester
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return null;

            // Retrieve student's enrollment table
            var enrollmentTableSpec = new EnrollmentTableWithSemesterAndStudentSpecification(stuId, semester.Id);
            var enrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdSpecificationAsync(enrollmentTableSpec);

            // Retrieve enrolled courses
            int count;
            var enrollmentsSpec = new EnrollmentsWithCoursesSpecification(enrollmentTable?.Id, null);
            var enrolledCourses = enrollmentTable != null
                ? _unitOfWork.Repository<Enrollment>().GetAllSpecification(enrollmentsSpec, out count)
                : new List<Enrollment>();

            // Get teacher's schedules for the department
            var teacherSchedulesSpec = new TeacherScheduleSpecification(student.DepartmentId);
            var teacherSchedules = _unitOfWork.Repository<TeacherSchedule>().GetAllSpecification(teacherSchedulesSpec, out count).ToList();

            // Extract enrolled course IDs
            var enrolledCourseIds = enrolledCourses.Select(e => e.CourseId).ToList();

            // Retrieve schedules for available courses
            var allCourseIds = teacherSchedules.Select(s => s.CourseId).ToList();
            var schedules = await _unitOfWork._scheduleRepo.GetScheduleByManycoursesAsync(allCourseIds, semester.Id);
            if (schedules is null) return null;

            // Filter out courses that do not meet the PreviousCourseNotTaken or have already been passed
            schedules = schedules
                .Where(schedule =>
                {
                    bool isPreviousCourseNotTaken = schedule.Course.PreviousCourseId.HasValue &&
                                                   !enrolledCourseIds.Contains(schedule.Course.PreviousCourseId.Value);
                    bool isAlreadyPassed = enrolledCourses.Any(e => e.CourseId == schedule.CourseId &&
                                                                   (e.IsPassAtCourse ?? false));
                    return !isPreviousCourseNotTaken && !isAlreadyPassed;
                })
                .ToList();

            // Map schedules to DTO with IsEnrolled flag
            var scheduleMapped = schedules.Select(s =>
            {
                var teacherNames = teacherSchedules
                    .Where(ts => ts.CourseId == s.CourseId && ts.Teacher != null)
                    .Select(ts => ts.Teacher.FullName)
                    .Distinct()
                    .ToList();

                return new ReadScheduleDto
                {
                    CourseId = s.CourseId,
                    SemesterName = s.Semester.Name,
                    CourseName = s.Course.Name,
                    TeachersName = string.Join(", ", teacherNames),
                    Duration = s.Course.Duration,
                    Hours = s.Course.CreditHours,
                    Day = s.Day,
                    ExamDate = s.ExamDate,
                    ExamEnd = s.ExamEnd,
                    ExamLocation = s.ExamLocation,
                    ExamStart = s.ExamStart,
                    LectureEnd = s.LectureEnd,
                    LectureLocation = s.LectureLocation,
                    LectureStart = s.LectureStart,
                    IsEnrolled = enrolledCourseIds.Contains(s.CourseId) // Check if the course is enrolled
                };
            }).ToList();

            if (scheduleMapped is null || !scheduleMapped.Any()) return null;

            // Calculate available hours
            var availableHours = (student.GPA >= 2 || enrolledCourses.Count() == 0) && student.CreditHours <= 108
                ? 18
                : student.GPA < 2
                    ? 12
                    : 21;

            // Map to final DTO
            var studentWithAvailableCourses = new ReadScheduleEnrollCourseDto
            {
                Id = stuId,
                EnrollmentTableId = enrollmentTable.Id,
                FullName = student.FullName,
                Level = student.Level,
                DepartmentName = student.Department.Name ?? "",
                GPA = student.GPA,
                Obtainedhours = student.CreditHours,
                AvailableHours = availableHours,
                AcademicGuide = student.Teacher?.FullName ?? "",
                Schedules = scheduleMapped
            };

            return new List<ReadScheduleEnrollCourseDto> { studentWithAvailableCourses };
        }

        public async Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetSchedulesByStudentId(Guid stuId)
        {
            // Retrieve the current semester
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return null;

            // Retrieve student's enrollment table in this semester
            var enrollmentTableSpec = new EnrollmentTableWithSemesterAndStudentSpecification(stuId, semester.Id);
            var enrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdSpecificationAsync(enrollmentTableSpec);
            if (enrollmentTable is null) return null;

            // Retrieve enrolled courses
            var enrollmentsSpec = new EnrollmentsWithCoursesSpecification(enrollmentTable.Id, semester.Id);
            var enrolledCourses = _unitOfWork.Repository<Enrollment>().GetAllSpecification(enrollmentsSpec, out int count);
            if (enrolledCourses is null || !enrolledCourses.Any()) return null;

            // Extract CourseIds for enrolled courses only
            var enrolledCourseIds = enrolledCourses.Select(e => e.CourseId).ToList();

            // Retrieve schedules for enrolled courses
            var schedules = await _unitOfWork._scheduleRepo.GetScheduleByManycoursesAsync(enrolledCourseIds, semester.Id);
            if (schedules is null || !schedules.Any()) return null;

            // Retrieve teacher schedules for enrolled courses to get teacher names
            var teacherSchedulesSpec = new TeacherScheduleSpecification(enrollmentTable.Student.DepartmentId);
            var teacherSchedules = _unitOfWork.Repository<TeacherSchedule>()
                .GetAllSpecification(teacherSchedulesSpec, out count)
                .Where(ts => enrolledCourseIds.Contains(ts.CourseId))
                .ToList();

            // Map schedules to DTO
            var scheduleMapped = schedules.Select(s =>
            {
                var teacherNames = teacherSchedules
                    .Where(ts => ts.CourseId == s.CourseId && ts.Teacher != null)
                    .Select(ts => ts.Teacher.FullName)
                    .Distinct()
                    .ToList();

                return new ReadScheduleDto
                {
                    CourseId = s.CourseId,
                    SemesterName = s.Semester.Name,
                    CourseName = s.Course.Name,
                    TeachersName = string.Join(", ", teacherNames),
                    Duration = s.Course.Duration,
                    Hours = s.Course.CreditHours,
                    Day = s.Day,
                    ExamDate = s.ExamDate,
                    ExamEnd = s.ExamEnd,
                    ExamLocation = s.ExamLocation,
                    ExamStart = s.ExamStart,
                    LectureEnd = s.LectureEnd,
                    LectureLocation = s.LectureLocation,
                    LectureStart = s.LectureStart,
                    IsEnrolled = true // All courses here are enrolled
                };
            }).ToList();

            if (scheduleMapped is null || !scheduleMapped.Any()) return null;

            // Map to final DTO
            var studentWithSchedule = new ReadScheduleEnrollCourseDto
            {
                Id = enrollmentTable.StudentId,
                EnrollmentTableId = enrollmentTable.Id,
                FullName = enrollmentTable.Student.FullName,
                Level = enrollmentTable.Student.Level,
                DepartmentName = enrollmentTable.Student.Department.Name ?? "",
                GPA = enrollmentTable.Student.GPA,
                Obtainedhours = enrollmentTable.Student.CreditHours,
                AvailableHours = enrollmentTable.Student.CreditHours <= 102 ? 18 : 21,
                AcademicGuide = enrollmentTable.Student.Teacher?.FullName ?? "",
                Schedules = scheduleMapped
            };

            return new List<ReadScheduleEnrollCourseDto> { studentWithSchedule };
        }

        public IReadOnlyList<ReadScheduleDto?> GetAllSchedules(ScheduleSpecificationParams specParams, out int count)
        {
            var scheduleSpec = ScheduleSpecification.ByParams(specParams);
            var schedules = _unitOfWork.Repository<Schedule>().GetAllSpecification(scheduleSpec, out count);
            if (schedules is null) return null;
            return _mapper.Map<IReadOnlyList<Schedule>, IReadOnlyList<ReadScheduleDto>>(schedules); ;
        }
    }
}
