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
        Task<ApiResponse> AssignOrUpdateGrade(UpdateEnrollmentDto updateEnrollmentDto);
        //Task<ApiResponse> AssignEnrollment(EnrollmentDto enrollmentDto);
        Task<ApiResponse> DeleteEnrollment(Guid enrollmentTableId, Guid courseId);
    }
}
