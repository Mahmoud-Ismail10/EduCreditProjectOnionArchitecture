using EduCredit.Core.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Configurations
{
    public class TeacherScheduleConfig : IEntityTypeConfiguration<TeacherSchedule>
    {
        public void Configure(EntityTypeBuilder<TeacherSchedule> builder)
        {
            builder.HasKey(ts => new { ts.TeacherId, ts.SemesterId, ts.CourseId });

            builder.HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSchedules)
                .HasForeignKey(ts => ts.TeacherId);

            builder.HasOne(ts => ts.Schedule)
                .WithMany(s => s.TeacherSchedules)
                .HasForeignKey(ts => new { ts.CourseId, ts.SemesterId });
        }
    }
}
