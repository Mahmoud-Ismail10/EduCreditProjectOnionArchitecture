using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ScheduleDTOs
{
    public class BaseScheduleDto
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required(ErrorMessage ="Day is required")]
        public Day Day { get; set; } // enum
        [Required(ErrorMessage = "Lecture Start Time is required")]
        public TimeOnly LectureStart { get; set; }
        [Required(ErrorMessage = "Lecture End Time is required")]
        public TimeOnly LectureEnd { get; set; }
        [Required(ErrorMessage = "Lecture Location is required")]
        public string LectureLocation { get; set; }
    }
}
