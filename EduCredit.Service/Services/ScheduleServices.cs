using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Core.Specifications.EnrollmentTableSpecifications;
using EduCredit.Core.Specifications.ScheduleSpecifications;
using EduCredit.Core.Specifications.SemesterCoursesSpecifications;
using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

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
        public async Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetStudentAvailableCourses(Guid stuId)
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
            if (enrollmentTable is null) return null;

            // Get available courses for the semester
            var semesterCoursesList = await _unitOfWork._semesterCourseRepo.GetSemesterCoursesBySemesterIdAsync(semester.Id);
            if (semesterCoursesList is null) return null;

            int count;
            // Retrieve enrolled courses
            var enrollmentsSpec = new EnrollmentsWithCoursesSpecification(enrollmentTable.Id);
            var enrolledCourses = _unitOfWork.Repository<Enrollment>().GetAllSpecification(enrollmentsSpec, out count);

            // Retrieve the schedule courses
            var scheduleSpec = new ScheduleSpecification();
            var schedules = _unitOfWork.Repository<Schedule>().GetAllSpecification(scheduleSpec, out count).ToList();
            if (schedules is null) return null;

            // Extract enrolled course IDs
            var enrolledCourseIds = enrolledCourses.Select(e => e.CourseId).ToList();

            // Filter out courses that do not meet the PreviousCourseNotTaken or have already been passed
            foreach (var course in schedules.ToList())
            {
                bool isPreviousCourseNotTaken = course.Course.PreviousCourseId.HasValue && !enrolledCourseIds.Contains(course.Course.PreviousCourseId.Value);
                bool isAlreadyPassed = enrolledCourses.Any(e => e.CourseId == course.CourseId && (e.IsPassAtCourse ?? false));

                if (isPreviousCourseNotTaken || isAlreadyPassed)
                {
                    schedules.Remove(course);
                }
            }

            // Map data to DTO
            var scheduleMapped = _mapper.Map<IReadOnlyList<Schedule>, List<ReadScheduleDto>>(schedules);
            if (scheduleMapped is null) return null;

            var studentWithAvailableCourses = new ReadScheduleEnrollCourseDto
            {
                Id = enrollmentTable.StudentId,
                StudentFullName = student.FullName,
                Level = student.Level,
                DepartmentName = student.Department.Name ?? "",
                GPA = student.GPA,
                Obtainedhours = student.CreditHours,
                AvailableHours = student.CreditHours <= 102 ? 18 : 21,
                NameOfGuide = student.Teacher?.FullName ?? "",
                semesterCourses = scheduleMapped,
            };

            return new List<ReadScheduleEnrollCourseDto> { studentWithAvailableCourses };
        }
        public async Task<IReadOnlyList<ReadScheduleEnrollCourseDto>?> GetScheduleById(Guid stuId)
        {
            var EnrollmentTableSpec = new EnrollmentTableWithSemesterAndStudentSpecification(stuId);
            var EnrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdSpecificationAsync(EnrollmentTableSpec);
            if (EnrollmentTable is null) return null;
            //هرجع ال student ب ال Id 
            var studentSpec = new StudentWithDepartmentAndGuideSpecification(EnrollmentTable.StudentId);
            var student = await _unitOfWork.Repository<Student>().GetByIdSpecificationAsync(studentSpec);
            if (student is null) return null;
            //EnrollmentTableId+CourseId =>get as schedule By CourseId
            var EnrollmentCourses =await _unitOfWork._courseRepo.GetCoursesByEnrollmentTableIdAsync(EnrollmentTable.Id);
            if (EnrollmentCourses is null) return null;
            int count;
            var schedulespec = new ScheduleSpecification(EnrollmentCourses);
            var schedule =  _unitOfWork.Repository<Schedule>().GetAllSpecification(schedulespec,out count);
            

            var scheduleMapped = _mapper.Map<IReadOnlyList<Schedule>, List<ReadScheduleDto>>(schedule);
          

               if (scheduleMapped is null) return null;
               var studentWithSchedule = new ReadScheduleEnrollCourseDto
               {
                   Id = EnrollmentTable.StudentId,
                   StudentFullName = student.FullName,
                   Level = student.Level,
                   DepartmentName = student.Department.Name ?? "",
                   GPA = student.GPA,
                   Obtainedhours = student.CreditHours,
                   AvailableHours = student.CreditHours <= 102 ? 18 : 21,
                   NameOfGuide =student.Teacher?.FullName ?? "",
                   semesterCourses = scheduleMapped,
               };
               return new List<ReadScheduleEnrollCourseDto> { studentWithSchedule };
        }
    }
}
