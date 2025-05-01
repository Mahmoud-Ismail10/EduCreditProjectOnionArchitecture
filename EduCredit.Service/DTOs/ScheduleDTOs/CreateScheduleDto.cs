using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ScheduleDTOs
{
    public class CreateScheduleDto : BaseScheduleDto
    {
        [Required]
        public Guid CourseId { get; set; }
        public Guid SemesterId { get; set; }

        [Required]
        public List<Guid> TeacherIds { get; set; } = new List<Guid>();
    }
}
