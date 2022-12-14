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
    public class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.Property(t => t.Name).HasMaxLength(50);
            builder.HasIndex(t => t.Name);

            builder.HasData(
                new League()
                {
                    Id = 20,
                    Name = "Default League Sample"
                }
            );
        }
    }
}
