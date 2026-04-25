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
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> entity)
        {
            entity.ToTable("RESERVATION");

            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

            entity.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(r => r.ReservedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.Property(r => r.ExpiresAt)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.HasIndex(r => r.UserId);
            entity.Property(r => r.UserId)
                .IsRequired();

            entity.HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(r => new { r.SeatId, r.ReservedAt });

            entity.HasIndex(r => r.SeatId);
            entity.Property(r => r.SeatId)
                .IsRequired();

            //entity.HasOne(r => r.Seat)
            //.WithOne(s => s.Reservation)
            //.HasForeignKey(r => r.SeatId)
            //.IsRequired()
            //.OnDelete(DeleteBehavior.Restrict);
        }
    }
}
