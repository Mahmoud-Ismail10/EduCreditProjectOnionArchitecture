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
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
              .ToTable("Students");

            builder.Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.NationalId)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.BirthDate)
                .HasConversion(v => v.ToDateTime(TimeOnly.MinValue),
                              v => DateOnly.FromDateTime(v))
                .IsRequired();

            /// Store gender in database as string and fetch it from DB as Gender
            builder.Property(s => s.Gender)
                .HasConversion(
                Gndr => Gndr.ToString(),
                Gndr => (Gender)Enum.Parse(typeof(Gender), Gndr));

            builder.Property(s => s.CreditHours)
                .HasColumnType("float")
                .HasDefaultValue(0.0f);

            builder.Property(s => s.GPA)
                .HasColumnType("float")
                .HasDefaultValue(0.0f);

            /// Store level in database as string and fetch it from DB as Level
            builder.Property(s => s.Level)
                .HasConversion(
                Lvl => Lvl.ToString(),
                Lvl => (Level)Enum.Parse(typeof(Level), Lvl));

            /// One-to-many: Between Student and Department
            builder.HasOne(s => s.Department)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            /// One-to-many: Between Student and Teacher (Guidance)
            builder.HasOne(s => s.Teacher)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
