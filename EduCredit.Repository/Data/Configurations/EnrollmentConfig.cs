using EduCredit.Core.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Configurations
{
    public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new {e.StudentId, e.CourseId});

            builder.Property(e => e.Grade)
                .HasColumnType("float");

            builder.Property(e => e.IsPassAtCourse)
                .HasColumnType("bit");

            builder.HasOne(e => e.Student)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.StudentId);

            builder.HasOne(tc => tc.Course)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
