using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Data.Configurations.Entities;
using Domain.Commons;

namespace Data
{
    public class FootballLeagueDbContext : AuditableFootballLeagueDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=FootballLeague_EfCore", 
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                })
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging(true); // It enables sensitive content from database workloads to be displayed even in the frontend(console app, browser by web app, etc)
        }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


            // Settings CreatedAt and UpdatedAt constraint columns -> No longer necessary, since the SaveChangesAsync() override method
            //do the work for us
            //modelBuilder.Entity<Team>().Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<League>().Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<Match>().Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<Coach>().Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");

            //modelBuilder.Entity<Team>().Property(t => t.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
            //modelBuilder.Entity<League>().Property(t => t.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
            //modelBuilder.Entity<Match>().Property(t => t.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
            //modelBuilder.Entity<Coach>().Property(t => t.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();


            modelBuilder.Entity<TeamsCoachesLeaguesView>().HasNoKey().ToView("TeamsCoachesLeagues");

            modelBuilder.ApplyConfiguration(new LeagueConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new CoachConfiguration());

            // Set all FK relationships should be restrict
            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                                                .SelectMany(x => x.GetForeignKeys())
                                                .Where(x => !x.IsOwnership && x.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //Configurations Temporal Tables, History Tables, for "Teams" real table
            modelBuilder.Entity<Team>().ToTable("Teams", b => b.IsTemporal());

        }

        //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        //{
        //    // Pre-convention model configuration goes here
        //    // Unicode (?)
        //    // Collation (?)
        //    //configurationBuilder.Properties<string>().AreUnicode(false).HaveMaxLength(50);
        //    configurationBuilder.Properties<string>().HaveMaxLength(50);

        //    base.ConfigureConventions(configurationBuilder);
        //}


        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Coach> Coaches { get; set; }

        public DbSet<TeamsCoachesLeaguesView> TeamsCoachesLeagues { get; set; }
    }
}
