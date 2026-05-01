
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfiguration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> entity)
        {
            entity.ToTable("EVENT");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.EventDate)
                .IsRequired()
                .HasColumnType("datetime2");

            entity.Property(e => e.Venue)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
