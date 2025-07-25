﻿using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Repositories.Contract
{
    public interface ITeacherRepo
    {
       Task< List<string>> GetSchedlesTeachers(List<Guid> teachers);
        IReadOnlyList<Teacher?> GetTeachersAreNotReachMaximumOfStudentsByDepartmentId(Guid? departmentId);
        Task<IReadOnlyList<Guid>> GetValidTeacherIds(List<Guid> teacherIds);
    }
}
