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
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(d => d.DepartmentHead)
                .WithOne(d => d.HeadofDepartment)
                .HasForeignKey<Department>(d => d.DepartmentHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull); /// Set null when delete another entity
        }
    }
}
