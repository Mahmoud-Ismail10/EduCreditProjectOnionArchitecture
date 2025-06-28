using EduCredit.Service.DTOs.EnrollmentTableDTOs;
using EduCredit.Service.Errors;

namespace EduCredit.Service.Services.Contract
{
    public interface IEnrollmentTableServices
    {
        Task<ApiResponse> CreateOrUpdateEnrollmentTable(CreateOrUpdateEnrollmentTableDto createOrUpdateEnrollmentTableDto);
        Task<CreateOrUpdateEnrollmentTableDto> GetEnrollmentTableByStudentId(Guid studentId);
        Task<ApiResponse> UpdateEnrollmentTableStatus(Guid EnrollmentTableId, UpdateEnrollmentTableDto dto);
        Task<int> EnrollmentTablesToDeleteAsync();
    }
}
