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
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Day Day { get; set; } // enum
        public TimeOnly Time { get; set; }
    }
}
