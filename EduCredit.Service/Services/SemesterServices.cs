using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.SemesterSpecifications;
using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class SemesterServices : ISemesterServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SemesterServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateSemester(CreateSemesterDto createSemesterDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            if (await _unitOfWork._semesterRepo.GetCurrentSemester() is null)
            {
                createSemesterDto.Year = createSemesterDto.EndDate.Year.ToString();

                var semester = _mapper.Map<CreateSemesterDto, Semester>(createSemesterDto);
                // Fetch all valid course IDs from DB
                var existingCourseIds = await _unitOfWork._courseRepo.GetValidCourseIds(createSemesterDto.CourseIds);
                // Check if all requested courses exist
                var missingCourses = createSemesterDto.CourseIds.Except(existingCourseIds).ToList();
                if (missingCourses.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiResponse(400, $"Courses not found: {string.Join(", ", missingCourses)}");
                }

                await _unitOfWork.Repository<Semester>().CreateAsync(semester);
                int resultSemester = await _unitOfWork.CompleteAsync();
                if (resultSemester <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiResponse(400, "Failed to create the semester!");
                }

                var schedules = createSemesterDto.CourseIds
                    .Select(courseId => new Schedule { SemesterId = semester.Id, CourseId = courseId })
                    .ToList();

                // Check schedules for duplicates
                var existingSchedules = await _unitOfWork.Repository<Schedule>()
                    .ListAsync(s => createSemesterDto.CourseIds.Contains(s.CourseId) && s.SemesterId == semester.Id);

                if (existingSchedules.Any())
                {
                    var duplicates = string.Join(", ", existingSchedules.Select(d => $"CourseId: {d.CourseId}, SemesterId: {d.SemesterId}"));
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiResponse(400, $"Schedules already exist for these courses in the semester: {duplicates}");
                }

                await _unitOfWork.Repository<Schedule>().CreateRangeAsync(schedules);
                int result = await _unitOfWork.CompleteAsync();

                if (result <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiResponse(400, "Failed to create the semester!");
                }
                await _unitOfWork.CommitTransactionAsync();
                return new ApiResponse(200, "Semester created successfully with assigned courses");
            }
            await _unitOfWork.RollbackTransactionAsync();
            return new ApiResponse(400, "Failed to create the semester, Because there is a current semester that has not ended!");
        }

        public async Task<ApiResponse> UpdateSemester(UpdateSemesterDto updateSemesterDto, Guid semesterId)
        {
            var spec = new SemesterWithCoursesSpecifications(semesterId);
            var semester = await _unitOfWork.Repository<Semester>().GetByIdSpecificationAsync(spec);
            if (semester is null) return new ApiResponse(404);

            // Fetch current schedules in this semester
            var currentSchedules = semester.Schedules.Select(e => e.CourseId).ToList();

            // Fetch the schedules that must be deleted from semester if exist
            var schedulesToRemove = semester.Schedules
                .Where(course => !updateSemesterDto.CourseIds.Contains(course.CourseId))
                .ToList();
            if (schedulesToRemove.Any())
                await _unitOfWork.Repository<Schedule>().DeleteRange(schedulesToRemove);


            // Update Schedule entries based on the new course list
            //var schedulesToRemove = updateSemesterDto.CourseIds
            //    .Where(courseId => !currentCourses.Contains(courseId))
            //    .Select(courseId => new Schedule { CourseId = courseId })
            //    .ToList();
            //if (schedulesToRemove.Any())
            //    await _unitOfWork.Repository<Schedule>()
            //        .DeleteRange(schedulesToRemove
            //        .Select(course => new Schedule { CourseId = course.CourseId })
            //        .ToList());

            //var coursesAndSchedulesToRemove = semester.SemesterCourses
            //    .Where(course => !updateSemesterDto.CourseIds.Contains(course.CourseId))
            //    .ToList();
            //if (coursesAndSchedulesToRemove.Any())
            //{
            //    await _unitOfWork.Repository<SemesterCourse>().DeleteRange(coursesAndSchedulesToRemove);
            //    await _unitOfWork.Repository<Schedule>()
            //        .DeleteRange(coursesAndSchedulesToRemove
            //        .Select(course => new Schedule { CourseId = course.CourseId })
            //        .ToList());
            //}

            int result = await _unitOfWork.CompleteAsync();
            if (result < 0) return new ApiResponse(400);
            updateSemesterDto.Year = updateSemesterDto.EndDate.Year.ToString();
            _mapper.Map(updateSemesterDto, semester);
            await _unitOfWork.Repository<Semester>().Update(semester);

            // Add new schedule to semester if they exist
            var newSchedules = updateSemesterDto.CourseIds
                .Where(courseId => !currentSchedules.Contains(courseId))
                .Select(courseId => new Schedule { SemesterId = semester.Id, CourseId = courseId })
                .ToList();
            if (newSchedules.Any())
            {
                await _unitOfWork.Repository<Schedule>().CreateRangeAsync(newSchedules);
            }

            result = await _unitOfWork.CompleteAsync();
            if (result < 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public async Task<ApiResponse> DeleteSemester(Guid semesterId)
        {
            var semester = await _unitOfWork.Repository<Semester>().GetByIdAsync(semesterId);
            if (semester is null) return new ApiResponse(404);

            await _unitOfWork.Repository<Semester>().Delete(semester);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public async Task<ReadSemesterDto?> GetCurrentSemester()
        {
            var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (currentSemester is null)
                return null;
            return _mapper.Map<Semester, ReadSemesterDto>(currentSemester);
        }

        public async Task<bool> IsEnrollmentOpenAsync()
        {
            var currentSemester = await _unitOfWork._semesterRepo.GetCurrentSemester();
            if (currentSemester is not null)
            {
                bool result = DateTime.UtcNow >= currentSemester.EnrollmentOpen && DateTime.UtcNow <= currentSemester.EnrollmentClose;
                return result;
            }
            return false;
        }

        public IReadOnlyList<ReadSemesterDto>? GetAllSemesters(SemesterSpecificationParams spec, out int count)
        {
            var specparams = new SemesterWithCoursesSpecifications(spec);
            var semesters = _unitOfWork.Repository<Semester>().GetAllSpecification(specparams, out count);
            if (semesters is not null)
            {
                var result =
                    semesters.Select(s => new ReadSemesterDto
                    {
                        Id = s.Id,
                        EndDate = s.EndDate,
                        StartDate = s.StartDate,
                        EnrollmentClose = s.EnrollmentClose,
                        EnrollmentOpen = s.EnrollmentOpen,
                        Name = s.Name,
                        Year = s.EndDate.Year.ToString(),
                        Schedules = s.Schedules.Select(sc => new ReadScheduleDto
                        {
                            CourseName = sc.Course.Name,
                            SemesterName = sc.Semester.Name,
                            TeachersName = string.Join(", ", sc.TeacherSchedules.Select(t => t.Teacher.FullName)),
                            CreditHours = sc.Course.CreditHours,
                            MinimumDegree = sc.Course.MinimumDegree,
                            PreviousCourse = sc.Course.PreviousCourse?.Name,
                            Duration = sc.Course.Duration,
                            Hours = sc.Course.CreditHours,
                            Day = sc.Day,
                            ExamDate = sc.ExamDate,
                            ExamEnd = sc.ExamEnd,
                            ExamLocation = sc.ExamLocation,
                            ExamStart = sc.ExamStart,
                            LectureEnd = sc.LectureEnd,
                            LectureLocation = sc.LectureLocation,
                            LectureStart = sc.LectureStart,
                        }).ToList()
                    }).ToList();
                return result;
            }
            return null;

        }

        public async Task<ReadSemesterDto?> GetSemesterByIdAsync(Guid id)
        {
            var spec = new SemesterWithCoursesSpecifications(id);
            var semester = await _unitOfWork.Repository<Semester>().GetByIdSpecificationAsync(spec);
            if (semester is not null)
            {
                var result = _mapper.Map<Semester, ReadSemesterDto>(semester);
                return result;
            }
            return null;
        }
    }
}