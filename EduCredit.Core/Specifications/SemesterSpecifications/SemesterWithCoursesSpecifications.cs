﻿using EduCredit.Core.Models;
using EduCredit.Core.Specifications.AdminSpecifications;
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
            Includes.Add(c => c.Schedules);
            ThenIncludes.Add("Schedules.TeacherSchedules");
            ThenIncludes.Add("Schedules.TeacherSchedules.Teacher");
            ThenIncludes.Add("Schedules.Course");
        }
        public SemesterWithCoursesSpecifications() : base()
        {
            Includes.Add(c => c.Schedules);
        }
        public SemesterWithCoursesSpecifications(SemesterSpecificationParams spec) : base(d =>
            (string.IsNullOrEmpty(spec.Search) || d.Name.ToLower().Contains(spec.Search.ToLower())))
        {
            Includes.Add(c => c.Schedules);
            ThenIncludes.Add("Schedules.Course");

            if (!string.IsNullOrEmpty(spec.Sort))
            {
                switch (spec.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(d => d.Name); break;
                    case "namedesc":
                        AddOrderByDesc(d => d.Name); break;
                    default:
                        AddOrderBy(d => d.StartDate); break;
                }
            }
                AddOrderBy(d => d.StartDate); 
            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
        }
    }
}
