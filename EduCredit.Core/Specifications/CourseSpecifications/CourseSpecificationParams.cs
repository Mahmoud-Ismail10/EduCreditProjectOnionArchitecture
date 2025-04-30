using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.CourseSpecifications
{
    public class CourseSpecificationParams : BaseSpecificationParams
    {
        public Guid? DepartmentId { get; set; }
        public Guid? PreviousCourseId { get; set; }
        public Guid? SemesterId { get; set; }
        public Guid? StudentId { get; set; }

    }
}
