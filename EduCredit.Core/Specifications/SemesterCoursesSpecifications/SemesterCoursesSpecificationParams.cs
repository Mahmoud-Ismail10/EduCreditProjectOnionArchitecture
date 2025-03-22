using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.SemesterCoursesSpecifications
{
    public class SemesterCoursesSpecificationParams : BaseSpecificationParams
    {
        public Guid? SemesterId { get; set; }
        public Guid? CourseId { get; set; }
    }
}
