using EduCredit.Core.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Configurations
{
    public class TeacherScheduleConfig : IEntityTypeConfiguration<TeacherSchedule>
    {
        public void Configure(EntityTypeBuilder<TeacherSchedule> builder)
        {
            builder.HasKey(ts => new { ts.TeacherId, ts.ScheduleId });
        }
    }
}
