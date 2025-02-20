using EduCredit.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Configurations
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CreditHours)
                .HasColumnType("float");

            builder.Property(c => c.MinimumDegree)
                .HasColumnType("float");

            builder.Property(c => c.ExamDate)
                .HasColumnType("datetime");

            /// Self-reference to represent the previous course
            builder.HasOne(c => c.PreviousCourse)
                .WithOne()
                .HasForeignKey<Course>(c => c.PreviousCourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            /// One-to-many: Between Course and Department
            builder.HasOne(c => c.Department)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
