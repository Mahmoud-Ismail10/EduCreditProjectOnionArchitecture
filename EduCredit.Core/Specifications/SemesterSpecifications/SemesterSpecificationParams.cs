using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.SemesterSpecifications
{
    public class SemesterSpecificationParams : BaseSpecificationParams
    {
        public Guid? StudentId { get; set; }
    }
}
