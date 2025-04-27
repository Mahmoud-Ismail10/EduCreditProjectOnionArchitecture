using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.ScheduleSpecifications
{
    public class ScheduleSpecification : BaseSpecifications<Schedule>
    {
        public ScheduleSpecification(ScheduleSpecificationParams spec)
            : base(s => s.CourseId == spec.CourseId)
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.TeacherSchedules);
        }
        public ScheduleSpecification()
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.TeacherSchedules);
        }
        public ScheduleSpecification(IReadOnlyList<Guid> courseIds)
            : base(s => courseIds.Contains(s.CourseId)) 
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.TeacherSchedules);
        }
        public ScheduleSpecification(Guid courseId)
            : base(s => s.CourseId == courseId)
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.TeacherSchedules);
            ThenIncludes.Add("TeacherSchedules.Teacher");
        }
    }

}
