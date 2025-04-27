using EduCredit.Service.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.StudentDTOs
{
    public class UpdateStudentDto : BaseRegisterDto
    {
        [Required]
        public Guid DepartmentId { get; set; }

    }
}
