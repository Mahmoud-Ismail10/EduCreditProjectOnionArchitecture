using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
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
            if (await _unitOfWork._semesterRepo.GetCurrentSemester() is null)
            {
                createSemesterDto.Year = createSemesterDto.EndDate.Year.ToString();

                var semester = _mapper.Map<CreateSemesterDto, Semester>(createSemesterDto);
                // Fetch all valid course IDs from DB
                var existingCourseIds = await _unitOfWork._courseRepo.GetValidCourseIds(createSemesterDto.CourseIds);
                // Check if all requested courses exist
                var missingCourses = createSemesterDto.CourseIds.Except(existingCourseIds).ToList();
                if (missingCourses.Any())
                    return new ApiResponse(400, $"Courses not found: {string.Join(", ", missingCourses)}");

                semester.SemesterCourses = createSemesterDto.CourseIds
                    .Select(courseId => new SemesterCourse { SemesterId = semester.Id, CourseId = courseId })
                    .ToList();

                await _unitOfWork.Repository<Semester>().CreateAsync(semester);
                int result = await _unitOfWork.CompleteAsync();

                if (result <= 0) return new ApiResponse(400, "Failed to create the semester!");
                return new ApiResponse(200, "Semester created successfully with assigned courses");
            }
            return new ApiResponse(400, "Failed to create the semester, Because there is a current semester that has not ended!");
        }

        public async Task<ApiResponse> UpdateSemester(UpdateSemesterDto updateSemesterDto, Guid semesterId)
        {
            var semester = await _unitOfWork.Repository<Semester>().GetByIdAsync(semesterId);
            if (semester is null) return new ApiResponse(404);

            var newSemester = _mapper.Map(updateSemesterDto, semester);
            await _unitOfWork.Repository<Semester>().Update(newSemester);
            int result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return new ApiResponse(400);
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
            return _mapper.Map<Semester, ReadSemesterDto?>(currentSemester);
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


    }
}