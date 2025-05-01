using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.ScheduleSpecifications
{
    public class ScheduleSpecificationParams : BaseSpecificationParams
    {
        public Guid? DepartmentId { get; set; }
        public Guid? TeacherId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? SemesterId { get; set; }
    }
}
