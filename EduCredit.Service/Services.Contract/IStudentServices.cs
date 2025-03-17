using EduCredit.Core.Specifications.StudentSpecifications;
using EduCredit.Service.DTOs.StudentDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IStudentServices
    {
        IReadOnlyList<ReadStudentDto?> GetAllStudents(StudentSpecificationParams specParams, out int count);
        Task<ReadStudentDto?> GetStudentByIdAsync(Guid id);
        Task<ApiResponse> UpdateStudentAsync(UpdateStudentDto updateStudentDto, Guid id);
        Task<ApiResponse> DeleteStudnetAsync(Guid id);
    }
}
