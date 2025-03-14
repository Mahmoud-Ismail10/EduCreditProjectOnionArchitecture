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
        Task<ApiResponse> AssignEnrollment(CreateEnrollmentDto createEnrollmentDto);
        Task<ApiResponse> DeleteEnrollment(Guid enrollmentTableId, Guid courseId);
    }
}
