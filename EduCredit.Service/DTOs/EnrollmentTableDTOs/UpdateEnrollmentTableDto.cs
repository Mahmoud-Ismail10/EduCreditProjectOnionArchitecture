using EduCredit.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentTableDTOs
{
    public class UpdateEnrollmentTableDto
    {
        public string? GuideNotes { get; set; }
        public Status? Status { get; set; }
    }
}
