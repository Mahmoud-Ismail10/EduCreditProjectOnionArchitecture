using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.SemesterDTOs
{
    public class BaseSemesterDto
    {
        [Required(ErrorMessage = "Semester Name is required")]
        [MinLength(3, ErrorMessage = "Semester Name must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Semester Name cannot exceed 50 characters")]
        [RegularExpression(pattern: "^[A-Za-z0-9\\s]{3,}$", ErrorMessage = "Semester Name must contain only letters, numbers, and spaces")]
        public string Name { get; set; }

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
