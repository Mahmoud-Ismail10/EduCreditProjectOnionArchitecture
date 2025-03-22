using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.SemesterDTOs
{
    public class CreateSemesterDto : BaseSemesterDto
    {
        [Required(ErrorMessage = "At least one course must be assigned to the semester")]
        public List<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}
