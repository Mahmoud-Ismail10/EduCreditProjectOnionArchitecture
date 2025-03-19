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
    public class SemesterCoursesConfig : IEntityTypeConfiguration<SemesterCourse>
    {
        public void Configure(EntityTypeBuilder<SemesterCourse> builder)
        {
            builder.HasKey(e => new { e.SemesterId, e.CourseId });
        }
    }
}
