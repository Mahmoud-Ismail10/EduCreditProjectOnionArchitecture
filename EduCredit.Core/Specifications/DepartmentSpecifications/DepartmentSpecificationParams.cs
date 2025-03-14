using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.DepartmentSpecifications
{
    public class DepartmentSpecificationParams : BaseSpecificationParams
    {
        public Guid? DepartmentHeadId { get; set; }

    }
}
