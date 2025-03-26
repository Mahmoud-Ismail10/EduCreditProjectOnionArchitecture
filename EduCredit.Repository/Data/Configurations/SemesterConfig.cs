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
    public class SemesterConfig : IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.StartDate)
              .HasConversion(v => v.ToDateTime(TimeOnly.MinValue),
                             v => DateOnly.FromDateTime(v))
              .IsRequired();

            builder.Property(s => s.EndDate)
              .HasConversion(v => v.ToDateTime(TimeOnly.MinValue),
                             v => DateOnly.FromDateTime(v))
              .IsRequired();
            
            builder.Property(s => s.EnrollmentOpen)
                .HasColumnType("datetime2")
                .IsRequired();
            
            builder.Property(s => s.EnrollmentClose)
                .HasColumnType("datetime2")
                .IsRequired();
        }
    }   
}
