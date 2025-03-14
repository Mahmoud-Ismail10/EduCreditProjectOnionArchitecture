using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Service.DTOs.DepartmentDTOs;
using EduCredit.Service.DTOs.EnrollmentDTOs;
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
            enrollment.Percentage = (updateEnrollmentDto.Grade / (course.CreditHours * 100)) * 100;
            if (enrollment.Grade >= course.MinimumDegree)
                updateEnrollmentDto.IsPassAtCourse = true;
            else
                updateEnrollmentDto.IsPassAtCourse = false;
            if (enrollment.Percentage >= 90)
                enrollment.Appreciation = Appreciation.Aplus;
            else if (enrollment.Percentage >= 85)
                enrollment.Appreciation = Appreciation.A;
            else if (enrollment.Percentage >= 80)
                enrollment.Appreciation = Appreciation.Bplus;
            else if (enrollment.Percentage >= 75)
                enrollment.Appreciation = Appreciation.B;
            else if (enrollment.Percentage >= 70)
                enrollment.Appreciation = Appreciation.Cplus;
            else if (enrollment.Percentage >= 65)
                enrollment.Appreciation = Appreciation.C;
            else if (enrollment.Percentage >= 60)
                enrollment.Appreciation = Appreciation.D;
            else
                enrollment.Appreciation = Appreciation.F;

            await _unitOfWork.Repository<Enrollment>().Update(enrollment);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public async Task<ApiResponse> AssignEnrollment(CreateEnrollmentDto createEnrollmentDto)
        {
            Enrollment enrollment = _mapper.Map<CreateEnrollmentDto, Enrollment>(createEnrollmentDto);
            await _unitOfWork.Repository<Enrollment>().CreateAsync(enrollment);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(400);
            return new ApiResponse(200);
        }

        public async Task<ApiResponse> DeleteEnrollment(Guid enrollmentTableId, Guid courseId)
        {
            var enrollment = await _unitOfWork._enrollmentRepo.GetEnrollmentByIdsAsync(enrollmentTableId, courseId);
            if (enrollment is null) return new ApiResponse(404);

            await _unitOfWork.Repository<Enrollment>().Delete(enrollment);
            int result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return new ApiResponse(200);
            return new ApiResponse(400);
        }
    }
}
