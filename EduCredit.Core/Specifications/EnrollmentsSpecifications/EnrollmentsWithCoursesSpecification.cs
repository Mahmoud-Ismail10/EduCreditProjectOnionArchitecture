using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.EnrollmentsSpecifications
{
    public class EnrollmentsWithCoursesSpecification : BaseSpecifications<Enrollment>
    {
        public EnrollmentsWithCoursesSpecification(Guid EnrollmentTableId)
            : base(e => e.EnrollmentTableId== EnrollmentTableId)
        {
            Includes.Add(e => e.Course);
        }
        public EnrollmentsWithCoursesSpecification()
         : base(e => e.IsPassAtCourse == true) { }
    }
    
}
