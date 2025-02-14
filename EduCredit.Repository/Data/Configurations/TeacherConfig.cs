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
    public class TeacherConfig : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.NationalId)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.BirthDate)
                .HasColumnType("date");

            builder.Property(t => t.Gender)
                .HasMaxLength(20);

            builder.Property(t => t.AppointmentDate)
                .HasColumnType("date");

            builder.HasOne(t => t.Department)
                .WithMany(t => t.Teachers)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
