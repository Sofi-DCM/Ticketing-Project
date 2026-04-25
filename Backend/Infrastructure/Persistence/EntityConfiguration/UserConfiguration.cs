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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("USER");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();

            entity.HasIndex(u => u.Name)
                  .IsUnique();
            entity.Property(d => d.Name)
                  .IsRequired()          
                  .HasMaxLength(50);

            entity.HasIndex(u => u.Email)
                  .IsUnique();
            entity.Property(d => d.Email)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(u => u.PasswordHash)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasData(

                //Admin -> contraseña : 1234
                new User { Id = 1, Name = "Admin", Email = "admin@gmail.com", PasswordHash = "$2a$12$JsxSgRNoJhtUIxlFO3YfWu.v6.yrLziZ/D/PEZK9oxTrEvJk9.nL2" }
                
                );
        }
    }
}
