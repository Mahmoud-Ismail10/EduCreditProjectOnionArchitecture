using EduCredit.Core.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Configurations
{
    internal class UserCourseGroupConfig : IEntityTypeConfiguration<UserCourseGroup>
    {
        public void Configure(EntityTypeBuilder<UserCourseGroup> builder)
        {
            builder.HasKey(x => new { x.UserId, x.CourseId });

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId);
        }
    }
}
