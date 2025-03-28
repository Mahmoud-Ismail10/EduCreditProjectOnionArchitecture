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
            : base(s => s.CourseId == spec.CourseId && s.TeacherId == spec.TeacherId)
          
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.Teacher);
        }
        public ScheduleSpecification()
            //: base(s => semesterId== s.Course.SemesterCourses.FirstOrDefault().SemesterId)
            //:base(s => s.Course.SemesterCourses.Any(sc => sc.SemesterId == semesterId))
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.Teacher);
        }
        public ScheduleSpecification(IReadOnlyList<Guid> courseIds)
        : base(s => courseIds.Contains(s.CourseId)) 
        {
            Includes.Add(s => s.Course);
            Includes.Add(s => s.Teacher);
        }
    }

}
