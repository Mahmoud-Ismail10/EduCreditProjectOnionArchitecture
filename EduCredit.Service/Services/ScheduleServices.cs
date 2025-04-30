using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Core.Specifications.EnrollmentTableSpecifications;
using EduCredit.Core.Specifications.ScheduleSpecifications;
//using EduCredit.Core.Specifications.SemesterCoursesSpecifications;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Core.Specifications.TeacherSchedualeSpecifications;
using EduCredit.Service.DTOs.ScheduleDTOs;
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

        /// Under development
        public async Task<ApiResponse> AssignSchedule(CreateScheduleDto createScheduleDto)
        {
            ///// Check if course and teacher are exist or no
            //var course = await _unitOfWork.Repository<Course>().GetByIdAsync(createScheduleDto.CourseId);
            //if (course is null) return new ApiResponse(400, "Course not found!");

            ///// Check if all requested teachers exist
            //var existingTeacherIds = await _unitOfWork._teacherRepo.GetValidTeacherIds(createScheduleDto.TeacherIds);
            //var missingTeachers = createScheduleDto.TeacherIds.Except(existingTeacherIds).ToList();
            //if (missingTeachers.Any())
            //    return new ApiResponse(400, $"Teachers not found: {string.Join(", ", missingTeachers)}");

            ///// Check if Schedule is exist or no
            //var existingSchedule = await _unitOfWork._scheduleRepo.GetScheduleByCourseIdAsync(createScheduleDto.CourseId);
            //if (existingSchedule is not null) return new ApiResponse(400, "Schedule already exists for this course!");

            /// Mapping data
            //var newSchedule = _mapper.Map<CreateScheduleDto, Schedule>(createScheduleDto);

            //newSchedule.TeacherSchedules = createScheduleDto.TeacherIds
            //        .Select(teacherId => new TeacherSchedule { ScheduleId = Id, TeacherId = teacherId })
            //        .ToList();

            ///// Create Schedule
            //await _unitOfWork.Repository<Schedule>().CreateAsync(newSchedule);

            ///// Save in DB
            //int result = await _unitOfWork.CompleteAsync();
            //if (result <= 0) return new ApiResponse(400, "Failed to assign schedule!");
            return new ApiResponse(200, "The schedule was successfully assigned");
        }

        public async Task<ReadScheduleDto?> GetSchedule(Guid CourseId)
        {
            var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            var scheduleSpec = new ScheduleSpecification(CourseId, currentSemester.Id);
            var scheduleSpecList = await _unitOfWork.Repository<Schedule>().GetByIdSpecificationAsync(scheduleSpec);
            if (scheduleSpecList is null) return null;
            var scheduleMapped = new ReadScheduleDto
            {
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
            /// Check if current semester is exist or no
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return new ApiResponse(404, "There is no current semester!");

            /// Check if schedule is exist or no
            var spec = new ScheduleSpecification(CourseId, semester.Id);
            var scheduleSpecList = await _unitOfWork.Repository<Schedule>().GetByIdSpecificationAsync(spec);
            if (scheduleSpecList is null) return new ApiResponse(404);

            /// Delete all the old teachers
            await _unitOfWork.Repository<TeacherSchedule>().DeleteRange(scheduleSpecList.TeacherSchedules.ToList());

            // Mapping data into existing schedule
            _mapper.Map(updateScheduleDto, scheduleSpecList);

            // Get current semester
            var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (currentSemester is null) return new ApiResponse(404, "There is no current semester");

            // Get existing teacher schedules for this schedule in the current semester
            var existingTeacherSchedules = await _unitOfWork._teacherScheduleRepo.GetTeacherSchedulesByScheduleIdAsync(CourseId, semester.Id);
            var existingTeacherIds = existingTeacherSchedules.Select(x => x).ToList();

            var distinctTeacherIds = updateScheduleDto.TeacherIds.Distinct().ToList();

            // 1. Identify teachers to remove ( موجودين في الداتا بيز بس مش مبعوتين دلوقتي )
            var teachersToRemove = existingTeacherSchedules
                .Where(ts => !distinctTeacherIds.Contains(ts))
                .ToList();
            /// Mapping data
            var newSchedule = _mapper.Map(updateScheduleDto, scheduleSpecList);

            // 2. Identify teachers to add ( مبعوتين دلوقتي ومش موجودين في الداتا بيز )
            var teachersToAdd = distinctTeacherIds
                .Where(id => !existingTeacherIds.Contains(id))
                .Select(teacherId => new TeacherSchedule
                {
                    TeacherId = teacherId
                })
                .ToList();
            newSchedule.TeacherSchedules = updateScheduleDto.TeacherIds
                    .Select(teacherId => new TeacherSchedule { SemesterId = scheduleSpecList.SemesterId, CourseId = scheduleSpecList.CourseId, TeacherId = teacherId })
                    .ToList();

            // 3. Remove old teacher schedules
            if (teachersToRemove.Any())
            {
                // هنا هنعمل تحويل لعناصر teachersToRemove بحيث تكون كائنات من نوع TeacherSchedule
                var teacherSchedulesToRemove = teachersToRemove.Select(ts => new TeacherSchedule
                {
                    TeacherId = ts,
                    CourseId = CourseId,
                    SemesterId = semester.Id
                }).ToList();

                await _unitOfWork.Repository<TeacherSchedule>().DeleteRange(teacherSchedulesToRemove);
            }


            // 4. Add new teacher schedules
            if (teachersToAdd.Any())
                await _unitOfWork.Repository<TeacherSchedule>().CreateRangeAsync(teachersToAdd);

            // 5. Update basic schedule info
            await _unitOfWork.Repository<Schedule>().Update(scheduleSpecList);

            // 6. Save changes
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400, "Failed to save schedule!");

            return new ApiResponse(200);
        }

        public async Task<ApiResponse> DeleteSchedule(Guid CourseId)
        {
            /// Check if current semester is exist or no
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return new ApiResponse(404, "There is no current semester!");

            /// Check if schedule is exist or no
            var spec = new ScheduleSpecification(CourseId, semester.Id);
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

            //// Get available courses for the semester
            //var semesterCoursesList = await _unitOfWork._semesterCourseRepo.GetSemesterCoursesBySemesterIdAndDepartmentIdAsync(semester.Id,student.DepartmentId);
            //if (semesterCoursesList is null) return null;

            int count;
            // Retrieve enrolled courses
            var enrollmentsSpec = new EnrollmentsWithCoursesSpecification(enrollmentTable.Id, null);
            var enrolledCourses = _unitOfWork.Repository<Enrollment>().GetAllSpecification(enrollmentsSpec, out count);

            // Retrieve the schedule courses
            //var scheduleSpec = new ScheduleSpecification();
            //var schedules = _unitOfWork.Repository<Schedule>().GetAllSpecification(scheduleSpec, out count).ToList();


            //if (schedules is null) return null;
            var TeacherSchedulesSpec = new TeacherScheduleSpecification(student.DepartmentId); /// student.DepartmentId, semester.Id
            var teacherSchedules = _unitOfWork.Repository<TeacherSchedule>().GetAllSpecification(TeacherSchedulesSpec, out count).ToList();

            // Extract enrolled course IDs
            var enrolledCourseIds =teacherSchedules.Select(s=>s.CourseId).ToList();
           var schedules = await _unitOfWork._scheduleRepo.GetScheduleByManycoursesAsync(enrolledCourseIds, semester.Id);
          if(schedules is null) return null;    
            // Filter out courses that do not meet the PreviousCourseNotTaken or have already been passed
            foreach (var schedule in schedules)
            {
                bool isPreviousCourseNotTaken = schedule.Course.PreviousCourseId.HasValue && !enrolledCourseIds.Contains(schedule.Course.PreviousCourseId.Value);
                bool isAlreadyPassed = enrolledCourses.Any(e => e.CourseId == schedule.CourseId && (e.IsPassAtCourse ?? false));

                if (isPreviousCourseNotTaken || isAlreadyPassed)
                {
                    schedules.Remove(schedule);

                }
            }
            //var Teachers = schedules.SelectMany(s => s.TeacherSchedules.Select(s => s.TeacherId)).ToList();
            //var TeacherNames = await _unitOfWork._teacherRepo.GetSchedlesTeachers(Teachers);
            var availableHours = (student.GPA >= 2 ||enrolledCourses.Count()==0)  && student.CreditHours <= 108 ? 18 :student.GPA < 2 ? 12:21;
            //// Map data to DTO
            //var scheduleMapped = _mapper.Map<IReadOnlyList<Schedule>, List<ReadScheduleDto>>(schedules);
            var scheduleMapped =
                 schedules.Select(s => new ReadScheduleDto
                 {
                     SemesterName = s.Semester.Name,
                     CourseName = s.Course.Name,
                     TeachersName = string.Join(", ",teacherSchedules?
                                            .Where(s => s.Teacher != null)
                                            .Select(t => t.Teacher.FullName)),
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
                 }).ToList();

            if (scheduleMapped is null) return null;

            var studentWithAvailableCourses = new ReadScheduleEnrollCourseDto
            {
                Id = enrollmentTable.StudentId,
                FullName = student.FullName,
                Level = student.Level,
                DepartmentName = student.Department.Name ?? "",
                GPA = student.GPA,
                Obtainedhours = student.CreditHours,
                AvailableHours = availableHours,
                AcademicGuide = student.Teacher?.FullName ?? "",
                Schedules = scheduleMapped,
            };

            return new List<ReadScheduleEnrollCourseDto> { studentWithAvailableCourses };
        }

        public async Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetSchedulesByStudentId(Guid stuId)
        {  
            //هرجع ال semester الحالي
            var semester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (semester is null) return null;
            // Retrieve student's enrollment table in this semester
            // Retrieve enrolled courses
            var enrollmentTableSpec = new EnrollmentTableWithSemesterAndStudentSpecification(stuId, semester.Id);
            var enrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdSpecificationAsync(enrollmentTableSpec);
            if (enrollmentTable is null)
                return null;
            
            var enrollmentsSpec = new EnrollmentsWithCoursesSpecification(enrollmentTable.Id, semester.Id);
            var enrolledCourses = _unitOfWork.Repository<Enrollment>().GetAllSpecification(enrollmentsSpec, out int count);
            if(enrolledCourses is null) return null;
            var TeacherSchedulesSpec = new TeacherScheduleSpecification(enrollmentTable.Student.DepartmentId);
            var teacherSchedules = _unitOfWork.Repository<TeacherSchedule>().GetAllSpecification(TeacherSchedulesSpec, out count).ToList();
            var courses = teacherSchedules.Select(s => s.CourseId).ToList();
            var schedules = await _unitOfWork._scheduleRepo.GetScheduleByManycoursesAsync(courses, semester.Id);

            //var scheduleMapped = _mapper.Map<IReadOnlyList<Schedule>, List<ReadScheduleDto>>(schedule);
            var scheduleMapped =
                schedules.Select(s => new ReadScheduleDto
                {
                    SemesterName = s.Semester.Name,
                    CourseName = s.Course.Name,
                    TeachersName = string.Join(", ", teacherSchedules?
                                            .Where(s => s.Teacher != null)
                                            .Select(t => t.Teacher.FullName)),
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
                }).ToList();

            if (scheduleMapped is null) return null;
            var studentWithSchedule = new ReadScheduleEnrollCourseDto
            {
                Id = enrollmentTable.StudentId,
                FullName = enrollmentTable.Student.FullName,
                Level = enrollmentTable.Student.Level,
                DepartmentName = enrollmentTable.Student.Department.Name ?? "",
                GPA = enrollmentTable.Student.GPA,
                Obtainedhours = enrollmentTable.Student.CreditHours,
                AvailableHours = enrollmentTable.Student.CreditHours <= 102 ? 18 : 21,
                AcademicGuide = enrollmentTable.Student.Teacher?.FullName ?? "",
                Schedules = scheduleMapped,
            };
            return new List<ReadScheduleEnrollCourseDto> { studentWithSchedule };
        }
    }
}
