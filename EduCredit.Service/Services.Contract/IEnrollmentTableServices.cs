using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.Errors;

namespace EduCredit.Service.Services.Contract
{
    public interface IEnrollmentTableServices
    {
        //Get Student Information and his Available Courses By Student Id   
        Task<IReadOnlyList<ReadEnrollmentTableDto>?> GetStudentAvailableCourses(string studentId);
        Task<ApiResponse> CreateOrUpdateEnrollmentTable(CreateOrUpdateEnrollmentTableDto createOrUpdateEnrollmentTableDto);

    }
}
