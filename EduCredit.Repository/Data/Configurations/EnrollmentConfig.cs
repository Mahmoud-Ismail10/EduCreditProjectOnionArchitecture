using EduCredit.Core.Enums;
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
            builder.HasKey(e => new {e.EnrollmentTableId, e.CourseId});

            builder.Property(e => e.Grade)
                .HasColumnType("float")
                .HasDefaultValue(0.0f);

            builder.Property(e => e.Percentage)
                .HasColumnType("float");

            builder.Property(e => e.Appreciation)
                .HasMaxLength(5)
                .HasConversion(
                App => App.ToString(),
                App => (Appreciation)Enum.Parse(typeof(Appreciation), App));

            builder.Property(e => e.IsPassAtCourse)
                .HasColumnType("bit");

            builder.HasOne(e => e.EnrollmentTable)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.EnrollmentTableId);

            builder.HasOne(tc => tc.Course)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
