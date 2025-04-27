using AutoMapper;
using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Core.Specifications.AdminSpecifications;
using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Service.DTOs.AdminDTOs;
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
        public IReadOnlyList<ReadEnrollmentDto> GetAllEnrollmentsAsync(EnrollmentSpecificationParams specParam, out int count)
        {
            var EnrollmentRepo = _unitOfWork.Repository<Enrollment>();
            var spec = new EnrollmentsWithCoursesSpecification(specParam);

            // Use asynchronous method if possible to avoid blocking IO operations
            var enrollments =  EnrollmentRepo.GetAllSpecification(spec, out count);

            if (enrollments is null || !enrollments.Any())
                return new List<ReadEnrollmentDto>();

            var student = enrollments.Select(e => e.EnrollmentTable.Student).FirstOrDefault();

            if (student is null)
                return new List<ReadEnrollmentDto>();

            // Calculate total credit hours and grade points
            var totalCreditHours = enrollments
                .Where(e => e.EnrollmentTable.StudentId == student.Id)
                .Sum(e => e.Course.CreditHours);

            var totalGradePoints = enrollments
                .Where(e => e.EnrollmentTable.StudentId == student.Id)
                .Sum(e => (e.Appreciation == Appreciation.F || !e.Appreciation.HasValue) ? 0 : e.Grade); // Setting Grade to 0 if below threshold

            // GPA percentage calculation
            var totalPercentageOfSemester = totalGradePoints / (totalCreditHours * 100f) * 100;
            var totalPercentage = (student.GPA / 4.0f) * 100;

            // Map Enrollments to ReadEnrollmentDto
            var enrollmentDtos = enrollments.Select(e => new ReadEnrollmentDto
            {
                StudentName = student.FullName,
                GPA = student.GPA,
                Obtainedhours = student.CreditHours,
                CreditHours = totalCreditHours,
                TotalObtainedhours = totalCreditHours - ((!e.IsPassAtCourse.HasValue||e.IsPassAtCourse.Value) ? 0 : e.Course.CreditHours),
                TotalPercentageOfSemester = totalPercentageOfSemester,
                TotalPercentage = totalPercentage,
                Appreciation = e.Appreciation,
                CourseId = e.CourseId,
                EnrollmentTableId = e.EnrollmentTableId,
                Grade = e.Grade,
                IsPassAtCourse = e.IsPassAtCourse,
                Percentage = e.Percentage,
            }).ToList();

            return enrollmentDtos;
        }

        //public IReadOnlyList<ReadEnrollmentDto?> GetAllEnrollmentsAsync(EnrollmentSpecificationParams specParam, out int count)
        //{
        //    var EnrollmentRepo = _unitOfWork.Repository<Enrollment>();
        //    var spec = new EnrollmentsWithCoursesSpecification(specParam);
        //    var Enrollments = EnrollmentRepo.GetAllSpecification(spec, out count);
        //    if (Enrollments is null)
        //        return null;
        //    var student = Enrollments.Select(e => e.EnrollmentTable.Student).FirstOrDefault();
        //    if (student is null)
        //        return null;
        //    var TotalCreditHours = Enrollments.Where(s=>s.EnrollmentTable.StudentId==student.Id).Sum(e => e.Course.CreditHours);
        //    var studentEnrollments = Enrollments.Where(s => s.EnrollmentTable.StudentId == student.Id).ToList();

        //    var totalGradePoints = studentEnrollments.Sum(e =>
        //         (e.Appreciation.HasValue&& e.Appreciation== Appreciation.F)? 0 : e.Grade); // Set Grade to 0 if below the threshold

        //    var totalCreditHours = studentEnrollments.Sum(s => s.Course.CreditHours);

        //    var totalPercentageOfSemester = (totalGradePoints / (totalCreditHours * 100)) * 100;
        //    var totalPercentage = (student.GPA/4.0f)*100;

        //    var enrollmentDto = Enrollments.Select(s => new ReadEnrollmentDto
        //    {
        //        StudentName = student.FullName,
        //        GPA = student.GPA,
        //        Obtainedhours = student.CreditHours,
        //        CreditHours = TotalCreditHours,
        //        TotalObtainedhours =TotalCreditHours- (s.IsPassAtCourse ==false? 0 : s.Course.CreditHours),
        //        TotalPercentageOfSemester = totalPercentageOfSemester,
        //        TotalPercentage= totalPercentage,
        //        Appreciation = s.Appreciation,
        //        CourseId = s.CourseId,
        //        EnrollmentTableId = s.EnrollmentTableId,
        //        Grade = s.Grade,
        //        IsPassAtCourse = s.IsPassAtCourse,
        //        Percentage = s.Percentage,
        //    }).ToList();
        //  //  var enrollmentDto = _mapper.Map<IReadOnlyList<Enrollment>, IReadOnlyList<ReadEnrollmentDto>>(Enrollments);
        //    return enrollmentDto;
        //}

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
