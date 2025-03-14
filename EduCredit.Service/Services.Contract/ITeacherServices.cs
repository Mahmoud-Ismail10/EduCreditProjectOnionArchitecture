using EduCredit.Core.Specifications.TeacherSpecefications;
using EduCredit.Service.DTOs.TeacherDTOs;
using EduCredit.Service.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ITeacherServices
    {
        IReadOnlyList<ReadTeacherDto?> GetAllTeachers(TeacherSpecificationParams specParams, out int count);
        Task<ReadTeacherDto?> GetTeacherByIdAsync(Guid id);
        Task<UpdateTeacherDto?> UpdateTeacherAsync(UpdateTeacherDto updateteacherDto, Guid id);
        Task<bool> DeleteTeacherAsync(Guid id);
    }
}
