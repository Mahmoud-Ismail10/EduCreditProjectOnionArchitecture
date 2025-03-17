using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.StudentSpecifications
{
    public class StudentSpecificationParams : BaseSpecificationParams
    {
        public Guid? DepartmentId { get; set; }
        public Guid? AcademicGuideId { get; set; }
    }
}
