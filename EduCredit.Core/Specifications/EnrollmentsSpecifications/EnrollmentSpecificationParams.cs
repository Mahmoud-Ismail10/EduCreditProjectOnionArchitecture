using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.EnrollmentsSpecifications
{
    public class EnrollmentSpecificationParams:BaseSpecificationParams
    {
        public Guid? enrollmentTableId { get; set; }
        public Guid? semesterId { get; set; }

    }
}
