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
    public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => new {s.TeacherId, s.CourseId});

            /// Store day in database as string and fetch it from DB as Day(Enum)
            builder.Property(t => t.Day)
                .HasConversion(
                Dy => Dy.ToString(),
                Dy => (Day)Enum.Parse(typeof(Day), Dy));

            builder.Property(s => s.LectureStart)
                .HasColumnType("time");
            
            builder.Property(s => s.LectureEnd)
                .HasColumnType("time");
            
            builder.Property(s => s.LectureLocation)
                .HasMaxLength(50);

            builder.Property(s => s.ExamDate)
                .HasColumnType("date");

            builder.Property(s => s.ExamStart)
                .HasColumnType("time");

            builder.Property(s => s.ExamEnd)
                .HasColumnType("time");

            builder.Property(s => s.ExamLocation)
                .HasMaxLength(50);

            builder.HasOne(s => s.Teacher)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.TeacherId);

            builder.HasOne(s => s.Course)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.CourseId);
        }
    }
}
