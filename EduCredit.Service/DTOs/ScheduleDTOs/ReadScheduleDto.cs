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
        public Guid CourseId { get; set; }
        public DateOnly? ExamDate { get; set; }
        public TimeOnly? ExamStart { get; set; }
        public TimeOnly? ExamEnd { get; set; }
        public string? ExamLocation { get; set; }
        public string TeachersName { get; set; }
        public string CourseName { get; set; }
        public string SemesterName { get; set; }
        public float Duration { get; set; }
        public float Hours { get; set; }
        public string? PreviousCourse { get; set; }
        public float CreditHours { get; set; }
        public float MinimumDegree { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
