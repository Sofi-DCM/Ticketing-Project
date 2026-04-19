using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfiguration
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> entity)
        {
            entity.ToTable("AUDIT_LOG");

            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

            entity.HasIndex(a => a.Action);
            entity.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(a => a.EntityType);
            entity.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(36);

            entity.HasIndex(a => a.CreatedAt);
            entity.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.HasIndex(a => a.UserId);
            entity.Property(a => a.UserId)
                .IsRequired(false);

            entity.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
