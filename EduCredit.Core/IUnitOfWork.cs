﻿using EduCredit.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// Generic Method
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        IEnrollmentTableRepo _enrollmentTableRepo { get; }
        IEnrollmentRepo _enrollmentRepo { get; }
        IScheduleRepo _scheduleRepo { get; }
        ISemesterRepo _semesterRepo { get; }
        ICourseRepo _courseRepo { get; }
        ITeacherRepo _teacherRepo { get; }
        IStudentRepo _studentRepo { get; }
        IDepartmentRepo _departmentRepo { get; }
        ITeacherScheduleRepo _teacherScheduleRepo { get; }
        IChatMessageRepo _chatMessageRepo { get; }
        IUserCourseGroupRepo _userCourseGroupRepo { get; }
    }
}
