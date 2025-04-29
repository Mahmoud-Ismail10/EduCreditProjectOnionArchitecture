//using EduCredit.Core.Models;
//using EduCredit.Core.Relations;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EduCredit.Core.Specifications.SemesterCoursesSpecifications
//{
    //public class SemesterCourseswithStudentSpecifications : BaseSpecifications<SemesterCourse>
    //{
    //    public SemesterCourseswithStudentSpecifications(SemesterCoursesSpecificationParams spec) : base(d =>
    //          (!spec.SemesterId.HasValue || d.SemesterId == spec.SemesterId)&&
    //          (!spec.CourseId.HasValue || d.CourseId == spec.CourseId))
    //    {
    //        Includes.Add(s => s.Semester);
    //        Includes.Add(s => s.Course);
    //    }
    //    public SemesterCourseswithStudentSpecifications()
    //    {
    //        Includes.Add(s => s.Semester);
    //        Includes.Add(s => s.Course);
    //    }

    //}
//}
