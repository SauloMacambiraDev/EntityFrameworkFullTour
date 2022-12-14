using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations.Entities
{
    public class CoachConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.Property(t => t.Name).HasMaxLength(50);
            // Composite Key
            builder.HasIndex(t => new { t.Name, t.TeamId }).IsUnique();

            builder.HasData(
                new Coach()
                {
                    Id = 20,
                    Name = "Trevoir Williams",
                    TeamId = 20
                },
                new Coach()
                {
                    Id = 21,
                    Name = "Trevoir Williams - Sample 1",
                    TeamId = 21
                },
                new Coach()
                {
                    Id = 22,
                    Name = "Trevoir Williams - Sample 2",
                    TeamId = 22
                }
            );
        }
    }
}
