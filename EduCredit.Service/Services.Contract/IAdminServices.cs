using EduCredit.Core.Enums;
using EduCredit.Core.Specifications.AdminSpecifications;
using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.AdminDTOs;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IAdminServices
    {
        IReadOnlyList<ReadAdminDto?> GetAllAdmins(AdminSpecificationParams specParams, out int count);
        Task<ReadAdminDto?> GetAdminByIdAsync(Guid id);
        Task<UpdateAdminDto?> UpdateAdminAsync(UpdateAdminDto updateadminDto, Guid id);
        Task<ApiResponse> DeleteAdminAsync(Guid id);
        Task<StatisticsDto> GetStatistics(Statistics statistics);
    }
}
