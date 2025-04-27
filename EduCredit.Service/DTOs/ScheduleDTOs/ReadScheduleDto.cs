using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.DTOs.ScheduleDTOs
{
    public class ReadScheduleDto : BaseScheduleDto
    {
        public Guid Id { get; set; }
        public DateOnly? ExamDate { get; set; }
        public TimeOnly? ExamStart { get; set; }
        public TimeOnly? ExamEnd { get; set; }
        public string? ExamLocation { get; set; }
        public List<string> TeachersName { get; set; }= new List<string>();
        public string CourseName { get; set; }
        public float Duration { get; set; }
        public float Hours { get; set; }
        public string? PreviousCourse { get; set; }
        public float CreditHours { get; set; }
        public float MinimumDegree { get; set; }

    }
}
