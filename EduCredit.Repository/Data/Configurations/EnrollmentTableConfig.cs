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
                Sts => (Status)Enum.Parse(typeof(Status), Sts));

            builder.Property(et => et.Session)
                .HasColumnType("date");

            builder.Property(et => et.Semester)
                .HasConversion(
                Smstr => Smstr.ToString(),
                Smstr => (Semester)Enum.Parse(typeof(Semester), Smstr));

            builder.HasOne(et => et.Student)
                .WithMany(et => et.EnrollmentTables)
                .HasForeignKey(et => et.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
