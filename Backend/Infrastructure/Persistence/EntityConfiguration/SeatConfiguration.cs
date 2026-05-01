
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfiguration
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> entity)
        {
            entity.ToTable("SEAT");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            entity.Property(s => s.RowIdentifier)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(s => s.SeatNumber)
                .IsRequired();

            entity.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(s => s.Version)
                .IsRequired()
                .HasDefaultValue(1)
                .IsConcurrencyToken();

            entity.HasOne(s => s.Sector)
                .WithMany(s => s.Seats)
                .HasForeignKey(s => s.SectorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(s => s.Reservations)
              .WithOne(r => r.Seat)
              .HasForeignKey(r => r.SeatId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
