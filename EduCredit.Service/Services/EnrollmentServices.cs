using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.DTOs.ScheduleDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AssignOrUpdateGrade(Guid enrollmentTableId, Guid courseId, UpdateEnrollmentDto updateEnrollmentDto)
        {
            var enrollment = await _unitOfWork._enrollmentRepo.GetEnrollmentByIdsAsync(enrollmentTableId, courseId);
            if (enrollment is null) return new ApiResponse(404);

            enrollment.Grade = updateEnrollmentDto.Grade;
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(courseId);
            enrollment.Percentage = (enrollment.Grade / (course.CreditHours * 100)) * 100;
            if (enrollment.Grade >= course.MinimumDegree)
                enrollment.IsPassAtCourse = true;
            else
                enrollment.IsPassAtCourse = false;
            enrollment.Appreciation = enrollment.Percentage switch
            {
                >= 90 => Appreciation.Aplus,
                >= 85 => Appreciation.A,
                >= 80 => Appreciation.Bplus,
                >= 75 => Appreciation.B,
                >= 70 => Appreciation.Cplus,
                >= 65 => Appreciation.C,
                >= 60 => Appreciation.D,
                _ => Appreciation.F
            };

            await _unitOfWork.Repository<Enrollment>().Update(enrollment);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        //public async Task<ApiResponse> AssignEnrollment(CreateEnrollmentDto createEnrollmentDto)
        //{
        //    /// Check if EnrollmentTable and course are exist or no
        //    var enrollmentTable = await _unitOfWork.Repository<EnrollmentTable>().GetByIdAsync(createEnrollmentDto.EnrollmentTableId);
        //    if (enrollmentTable is null) return new ApiResponse(400, "EnrollmentTable not found!");
        //    var course = await _unitOfWork.Repository<Course>().GetByIdAsync(createEnrollmentDto.CourseId);
        //    if (course is null) return new ApiResponse(400, "Course not found!");

        //    /// Check if Enrollment is exist or no
        //    var existingEnrollment = await _unitOfWork._enrollmentRepo.GetEnrollmentByIdsAsync(createEnrollmentDto.EnrollmentTableId, createEnrollmentDto.CourseId);
        //    if (existingEnrollment is not null) return new ApiResponse(400, "Enrollment already exists for this course!");

        //    /// Mapping data
        //    Enrollment enrollment = _mapper.Map<CreateEnrollmentDto, Enrollment>(createEnrollmentDto);

        //    /// Create Enrollment
        //    await _unitOfWork.Repository<Enrollment>().CreateAsync(enrollment);

        //    /// Save in DB
        //    int result = await _unitOfWork.CompleteAsync();
        //    if (result <= 0) return new ApiResponse(400, "Failed to assign enrollment!");
        //    return new ApiResponse(200, "The enrollment was successfully assigned");
        //}

        //public async Task<ApiResponse> DeleteEnrollment(Guid enrollmentTableId, Guid courseId)
        //{
        //    var enrollment = await _unitOfWork._enrollmentRepo.GetEnrollmentByIdsAsync(enrollmentTableId, courseId);
        //    if (enrollment is null) return new ApiResponse(404);

        //    await _unitOfWork.Repository<Enrollment>().Delete(enrollment);
        //    int result = await _unitOfWork.CompleteAsync();
        //    if (result <= 0) return new ApiResponse(200);
        //    return new ApiResponse(400);
        //}

        //public async Task<ReadEnrollmentDto?> GetEnrollment(Guid enrollmentTableId, Guid courseId)
        //{
        //    var enrollment = await _unitOfWork._enrollmentRepo.GetEnrollmentByIdsAsync(enrollmentTableId, courseId);
        //    if (enrollment is null) return null;
        //    var enrollmentDto = _mapper.Map<Enrollment, ReadEnrollmentDto>(enrollment);
        //    return enrollmentDto;
        //}
    }
}
