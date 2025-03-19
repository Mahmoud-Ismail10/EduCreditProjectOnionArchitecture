using EduCredit.Core.Enums;
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
    class EnrollmentTableConfig : IEntityTypeConfiguration<EnrollmentTable>
    {
        public void Configure(EntityTypeBuilder<EnrollmentTable> builder)
        {
            builder.Property(et => et.Status)
                .HasConversion(
                Sts => Sts.ToString(),
                Sts => (Status)Enum.Parse(typeof(Status), Sts))
                .IsRequired();

            builder.Property(e => e.StudentNotes)
                .HasMaxLength(500);

            builder.Property(e => e.GuideNotes)
                .HasMaxLength(500);

            /// One-to-Many: EnrollmentTable → Semester
            builder.HasOne(e => e.Semester)
                .WithMany(e => e.EnrollmentTables)
                .HasForeignKey(e => e.SemesterId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            /// One-to-Many: EnrollmentTable → Student
            builder.HasOne(et => et.Student)
                .WithMany(et => et.EnrollmentTables)
                .HasForeignKey(et => et.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if student is removed
        }
    }
}
