using EduCredit.Core.Enums;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.SemesterDTOs
{
    public class BaseSemesterDto
    {
        [Required(ErrorMessage = "Semester Type is required")]
        public SemesterType SemesterType { get; set; }

        public string? Year { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Enrollment Open is required")]
        public DateTime EnrollmentOpen { get; set; }

        [Required(ErrorMessage = "Enrollment Close is required")]
        public DateTime EnrollmentClose { get; set; }
    }
}
