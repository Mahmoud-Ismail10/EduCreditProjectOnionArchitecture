using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.TeacherSchedualeSpecifications
{
    public class TeacherScheduleSpecification : BaseSpecifications<TeacherSchedule>
    {
        public TeacherScheduleSpecification(Guid? DepartmentId):
            base(s => s.Schedule.Course.DepartmentId== DepartmentId)
        {
           
            Includes.Add(d => d.Teacher);
            Includes.Add(d => d.Schedule);
            ThenIncludes.Add("Schedule.Course");
            ThenIncludes.Add("Schedule.Semester");
        }

     //   public TeacherScheduleSpecification()
     //: base(e =>
     //    (!.HasValue || e.EnrollmentTableId == enrollmentTableId.Value) &&
     //    (!semesterId.HasValue || e.EnrollmentTable.SemesterId == semesterId.Value))
     //   {
     //       Includes.Add(e => e.Course);
     //   }
    }
}
