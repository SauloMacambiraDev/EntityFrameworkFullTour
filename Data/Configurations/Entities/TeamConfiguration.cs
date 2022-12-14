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
    //public class TeamSeedConfiguration : IEntityTypeConfiguration<Team>
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasMany(t => t.HomeMatches)
                  .WithOne(m => m.HomeTeam)
                  .HasForeignKey(t => t.HomeTeamId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.AwayMatches)
                   .WithOne(m => m.AwayTeam)
                   .HasForeignKey(t => t.AwayTeamId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Name).HasMaxLength(50);
            builder.HasIndex(t => t.Name).IsUnique();

            builder.HasData(
              new Team()
              {
                  Id = 20,
                  Name = "Trevoir Williams - Sample Team",
                  LeagueId = 20
              },
              new Team()
              {
                  Id = 21,
                  Name = "Trevoir Williams - Sample Team",
                  LeagueId = 20
              },
              new Team()
              {
                  Id = 22,
                  Name = "Trevoir Williams - Sample Team",
                  LeagueId = 20
              }
            );

           
        }
    }
}
