using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Relations
{
    public class Schedule
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Guid SemesterId { get; set; }
        public Semester Semester { get; set; }

        public Day? Day { get; set; } // enum
        public TimeOnly? LectureStart { get; set; }
        public TimeOnly? LectureEnd { get; set; }
        public string? LectureLocation { get; set; }
        public DateOnly? ExamDate { get; set; }
        public TimeOnly? ExamStart { get; set; }
        public TimeOnly? ExamEnd { get; set; }
        public string? ExamLocation { get; set; }
        
        /// Many-to-many: Between Schedule and Teacher (JoinTable)
        public ICollection<TeacherSchedule> TeacherSchedules { get; set; } = new HashSet<TeacherSchedule>();
    }
}
