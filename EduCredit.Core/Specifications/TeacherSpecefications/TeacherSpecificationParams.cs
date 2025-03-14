using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.TeacherSpecefications
{
    public class TeacherSpecificationParams
    {
        public string? Sort { get; set; }
        public Guid? DepartmentId { get; set; }

        private const int MaxPageSize = 10;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1; // make defualt value is 1
        public string? Search { get; set; }
    }
}
