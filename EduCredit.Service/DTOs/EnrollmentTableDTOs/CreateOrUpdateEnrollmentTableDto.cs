using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.EnrollmentTableDTOs
{
    public class CreateOrUpdateEnrollmentTableDto
    {
        public Status? Status { get; set; }
        public string? StudentNotes { get; set; }
        public string? GuideNotes { get; set; }
        public Guid? SemesterId { get; set; } // Foreign Key
        public Guid StudentId { get; set; } // Foreign Key
    }
}
