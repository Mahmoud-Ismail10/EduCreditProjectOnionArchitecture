using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Relations
{
    public class TeacherSchedule
    {
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
