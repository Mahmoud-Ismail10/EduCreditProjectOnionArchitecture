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

            builder.Property(s => s.Day)
                .HasConversion<int>(); /// Store as integer

            builder.Property(s => s.Time)
                .HasColumnType("time");

            builder.HasOne(s => s.Teacher)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.TeacherId);

            builder.HasOne(s => s.Course)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.CourseId);
        }
    }
}
