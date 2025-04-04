using EduCredit.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Repository.Data.Configurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(rt => rt.ExpiryDate)
                .IsRequired();

            builder.HasIndex(rt => rt.Token)
                .IsUnique();

            builder.HasOne(rt => rt.Person)
           .WithMany(p => p.RefreshTokens)
           .HasForeignKey(rt => rt.UserId)
           .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
