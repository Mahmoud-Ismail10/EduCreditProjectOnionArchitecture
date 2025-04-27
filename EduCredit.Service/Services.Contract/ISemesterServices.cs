    using EduCredit.Core.Specifications.SemesterSpecifications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.SemesterDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ISemesterServices
    {
        Task<ApiResponse> CreateSemester(CreateSemesterDto createSemesterDto);
        Task<ApiResponse> UpdateSemester(UpdateSemesterDto updateSemesterDto, Guid semesterId);
        Task<ApiResponse> DeleteSemester(Guid semesterId);
        Task<ReadSemesterDto?> GetCurrentSemester();
        Task<bool> IsEnrollmentOpenAsync();
        IReadOnlyList<ReadSemesterDto>? GetAllSemesters(SemesterSpecificationParams spec ,out int count);
        Task<ReadSemesterDto?> GetSemesterByIdAsync(Guid id);
    }
}
