﻿using EduCredit.Core.Chat;
using EduCredit.Core.Models;
using EduCredit.Core.Relations;
using EduCredit.Repository.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data
{
    public class EduCreditContext : IdentityDbContext<Person, IdentityRole<Guid>, Guid>
    {
        /// Using Dependancy Injection, and add service of dbcontext in program.cs
        public EduCreditContext(DbContextOptions<EduCreditContext> options) : base(options) { }

        /// When using parameter lass constuctor, must override OnConfiguring()
        //public EduCreditContext() { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("ConnectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<EnrollmentTable> EnrollmentTables { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<TeacherSchedule> TeacherSchedules { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserCourseGroup> UserCourseGroups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// Using it when inherate from identity dbcontext no dbcontext
            base.OnModelCreating(modelBuilder);

            /// Apply FluentAPI
            //modelBuilder.ApplyConfiguration(new CourseConfig());
            //modelBuilder.ApplyConfiguration(new DepartmentConfig());
            //modelBuilder.ApplyConfiguration(new EnrollmentConfig());
            //modelBuilder.ApplyConfiguration(new ScheduleConfig());
            //modelBuilder.ApplyConfiguration(new StudentConfig());
            //modelBuilder.ApplyConfiguration(new TeacherConfig());

            /// Execute all Configurations that implement from IEntityTypeConfiguration<>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
