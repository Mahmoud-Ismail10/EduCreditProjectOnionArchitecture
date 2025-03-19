using EduCredit.Core.Models;
using EduCredit.Core.Specifications.DepartmentSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.SemesterSpecifications
{
    public class SemesterWithCoursesSpecifications : BaseSpecifications<Semester>
    {
        /// for get one semester (with Criteria)
        public SemesterWithCoursesSpecifications(Guid id) : base(d => d.Id == id)
        {
            Includes.Add(c => c.SemesterCourses);
        }
    }
}
