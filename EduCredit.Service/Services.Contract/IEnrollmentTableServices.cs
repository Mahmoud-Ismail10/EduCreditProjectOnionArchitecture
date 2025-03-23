using EduCredit.Service.DTOs.EnrollmentTableDTOs;

namespace EduCredit.Service.Services.Contract
{
    public interface IEnrollmentTableServices
    {
        //Get Student Information and his Available Courses By Student Id   
        Task<IReadOnlyList<ReadEnrollmentTableDto>?> GetStudentAvailableCourses(string studentId);

    }
}
