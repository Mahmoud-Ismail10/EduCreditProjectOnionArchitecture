using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Specifications.ScheduleSpecifications
{
    public class ScheduleSpecification : BaseSpecifications<Schedule>
    {
        //public ScheduleSpecification(ScheduleSpecificationParams spec) : base(d =>
        //    (!spec.TeacherId.HasValue || d.TeacherSchedules.Any(sc => sc.TeacherId == spec.TeacherId.Value)) &&
        //    (!spec.SemesterId.HasValue || d.SemesterId == spec.SemesterId.Value) &&
        //    (!spec.DepartmentId.HasValue || d.Course.DepartmentId == spec.DepartmentId.Value) &&
        //    (!spec.CourseId.HasValue || d.CourseId == spec.CourseId.Value))
        //{
        //    Includes.Add(s => s.Course);
        //    Includes.Add(s => s.Semester);
        //    Includes.Add(s => s.TeacherSchedules);
        //    ThenIncludes.Add("TeacherSchedules.Teacher");
        //    ThenIncludes.Add("Course.Department");
        //}
     
        //public ScheduleSpecification(Guid CourseId, Guid SemesterId)
        //    : base(s => s.CourseId == CourseId && s.SemesterId == SemesterId)
        //{
        //    Includes.Add(s => s.Course);
        //    Includes.Add(s => s.Semester);
        //    Includes.Add(s => s.TeacherSchedules);
        //    ThenIncludes.Add("TeacherSchedules.Teacher");
        //}
        private ScheduleSpecification(Expression<Func<Schedule, bool>> criteria) : base(criteria)
        {
        }

        private ScheduleSpecification() : base(null)
        {
        }

        public static ScheduleSpecification ByParams(ScheduleSpecificationParams spec)
        {
            var specification = new ScheduleSpecification(s =>
                (!spec.TeacherId.HasValue || s.TeacherSchedules.Any(ts => ts.TeacherId == spec.TeacherId.Value)) &&
                (!spec.SemesterId.HasValue || s.SemesterId == spec.SemesterId.Value) &&
                (!spec.DepartmentId.HasValue || s.Course.DepartmentId == spec.DepartmentId.Value) &&
                (!spec.CourseId.HasValue || s.CourseId == spec.CourseId.Value)
            );

            specification.Includes.Add(s => s.Course);
            specification.Includes.Add(s => s.Semester);
            specification.Includes.Add(s => s.TeacherSchedules);
            specification.ThenIncludes.Add("TeacherSchedules.Teacher");
            specification.ThenIncludes.Add("Course.Department");

            return specification;
        }

        public static ScheduleSpecification All()
        {
            var specification = new ScheduleSpecification();

            specification.Includes.Add(s => s.Course);
            specification.Includes.Add(s => s.Semester);
            specification.Includes.Add(s => s.TeacherSchedules);

            return specification;
        }

        public static ScheduleSpecification ByCourseIds(IReadOnlyList<Guid> courseIds)
        {
            var specification = new ScheduleSpecification(s => courseIds.Contains(s.CourseId));

            specification.Includes.Add(s => s.Course);
            specification.Includes.Add(s => s.Semester);
            specification.Includes.Add(s => s.TeacherSchedules);

            return specification;
        }

        public static ScheduleSpecification ByCourseAndSemester(Guid courseId, Guid semesterId)
        {
            var specification = new ScheduleSpecification(s => s.CourseId == courseId && s.SemesterId == semesterId);

            specification.Includes.Add(s => s.Course);
            specification.Includes.Add(s => s.Semester);
            specification.Includes.Add(s => s.TeacherSchedules);
            specification.ThenIncludes.Add("TeacherSchedules.Teacher");

            return specification;
        }

        public static ScheduleSpecification ByTeacherAndSemester(Guid teacherId, Guid semesterId)
        {
            var specification = new ScheduleSpecification(s =>
                s.SemesterId == semesterId &&
                s.TeacherSchedules.Any(ts => ts.TeacherId == teacherId));

            specification.Includes.Add(s => s.Course);
            specification.Includes.Add(s => s.TeacherSchedules);
            specification.ThenIncludes.Add("Course.Enrollments.EnrollmentTable.Student");

            return specification;
        }
    

    }

}
