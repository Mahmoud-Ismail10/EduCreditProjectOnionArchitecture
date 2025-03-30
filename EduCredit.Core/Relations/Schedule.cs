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
        public Guid CourseId { get; set; } // Primary key

        public Day Day { get; set; } // enum
        public TimeOnly LectureStart { get; set; }
        public TimeOnly LectureEnd { get; set; }
        public string LectureLocation { get; set; }
        public DateOnly? ExamDate { get; set; }
        public TimeOnly? ExamStart { get; set; }
        public TimeOnly? ExamEnd { get; set; }
        public string? ExamLocation { get; set; }

        /// One-to-One: Between Schedule and Course
        public Course Course { get; set; }

        /// Many-to-many: Between Schedule and Teacher (JoinTable)
        public ICollection<TeacherSchedule> TeacherSchedules { get; set; } = new HashSet<TeacherSchedule>();
    }
}
