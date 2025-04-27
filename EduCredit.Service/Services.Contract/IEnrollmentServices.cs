using EduCredit.Core.Specifications.EnrollmentsSpecifications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.EnrollmentDTOs;
using EduCredit.Service.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IEnrollmentServices
    {
        Task<ApiResponse> AssignOrUpdateGrade(Guid enrollmentTableId, Guid courseId, UpdateEnrollmentDto updateEnrollmentDto);
        IReadOnlyList<ReadEnrollmentDto?> GetAllEnrollmentsAsync(EnrollmentSpecificationParams spec, out int count);
        //Task<ApiResponse> AssignEnrollment(CreateEnrollmentDto createEnrollmentDto);
        //Task<ReadEnrollmentDto?> GetEnrollment(Guid enrollmentTableId, Guid courseId);
        //Task<ApiResponse> DeleteEnrollment(Guid enrollmentTableId, Guid courseId);
    }
}
