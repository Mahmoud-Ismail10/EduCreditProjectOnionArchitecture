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
            builder.HasKey(s => s.Id); // Id as primary key

            /// Store day in database as string and fetch it from DB as Day(Enum)
            builder.Property(t => t.Day)
                .HasConversion(
                Dy => Dy.ToString(),
                Dy => (Day)Enum.Parse(typeof(Day), Dy))
                .IsRequired(false);

            builder.Property(s => s.LectureStart)
                   .HasConversion(v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
                             v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null);

            builder.Property(s => s.LectureEnd)
               .HasConversion(v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
                             v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null);

            builder.Property(s => s.LectureLocation)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(s => s.ExamDate)
                .HasConversion(v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                             v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null);

            builder.Property(s => s.ExamStart)
                .HasConversion(v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
                             v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null);

            builder.Property(s => s.ExamEnd)
                .HasConversion(v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
                             v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null);

            builder.Property(s => s.ExamLocation)
                .HasMaxLength(50)
                .IsRequired(false);

            /// Many-to-one: Between Schedule and Course
            builder.HasOne(d => d.Course)
                .WithMany(d => d.Schedules)
                .HasForeignKey(d => d.CourseId) // CourseId as foreign key
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
