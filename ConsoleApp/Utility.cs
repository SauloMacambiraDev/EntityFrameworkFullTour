using Data;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class Utility
    {
        public static async Task TeamsHistoryTemporalQueries(FootballLeagueDbContext context)
        {
            Console.WriteLine("1) TeamsQueries being executed..");
            var teamsHistory = await context.Teams.TemporalAll().ToListAsync();
            foreach (var th in teamsHistory)
            {
                Console.WriteLine($"Team: {th.Name}");
            }

            Console.WriteLine("2) TeamsQueries has ended");
        }

        public static async Task TeamsQueries(FootballLeagueDbContext context)
        {
            Console.WriteLine("1) TeamsQueries being executed..");
            var teams = await context.Teams.ToListAsync();
            foreach (var team in teams)
            {
                Console.WriteLine($"Team: {team.Name}");
            }

            Console.WriteLine("2) TeamsQueries has ended");
        }
        public static async Task ExecutingTransactionWithSavePoint(FootballLeagueDbContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                League league = new League()
                {
                    Name = "Audit Testing League"
                };
                context.Leagues.Add(league);
                //await context.SaveChangesAsync("Tester User Saulo");
                await context.SaveChangesAsync();

                await transaction.CreateSavepointAsync("SavedLeague");

                var teams = new List<Team>()
                {
                    new Team() {
                        Name = "Juventus",
                        LeagueId = league.Id,
                    },
                    new Team() {
                        Name = "AC Milan",
                        LeagueId = league.Id,
                    },
                    new Team() {
                        Name = "AS Roma",
                        League = league,
                    },

                };

                context.AddRange(teams);
                await context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                /*
                 * Rollback() is the default procedure in case the transaction instance didn't hit the Commit() method.
                 * Which means that, if an error ocurred while trying to add Teams, for example, the League previously
                 * added using the SaveChangesAsync() would be rolled back right away.
                 * Also, since the Rollback() is the default procedure, there is no need to specify: transaction.Rollback()
                 * on catch() block.
                */
                await transaction.RollbackToSavepointAsync("SavedLeague");
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task ExecutingDefaultTransaction(FootballLeagueDbContext context)
        {
            var transaction = context.Database.BeginTransaction();
            try
            {
                League league = new League()
                {
                    Name = "Audit Testing League"
                };
                context.Leagues.Add(league);
                await context.SaveChangesAsync();

                await AddTeamsWithLeagueId(league, context);
                //await context.SaveChangesAsync();

                transaction.Commit();
            } catch(Exception ex)
            {
                 /*
                  * Rollback() is the default procedure in case the transaction instance didn't hit the Commit() method.
                  * Which means that, if an error ocurred while trying to add Teams, for example, the League previously
                  * added using the SaveChangesAsync() would be rolled back right away.
                  * Also, since the Rollback() is the default procedure, there is no need to specify: transaction.Rollback()
                  * on catch() block.
                 */
                transaction.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task ExecuteNonQueryCommand(FootballLeagueDbContext context)
        {
            int teamId = 2;
            var affectedRows = await context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteTeamById {0}", teamId);

            int teamId2 = 5;
            var affectedRows2 = await context.Database.ExecuteSqlInterpolatedAsync($"EXEC sp_DeleteTeamById {teamId2}");


        }
        public static async Task ExecStoredProcedure(FootballLeagueDbContext context)
        {
            int teamId = 3;
            var result = await context.Coaches.FromSqlRaw("EXEC dbo.sp_sp_GetTeamCoach {0}", teamId).ToListAsync();
        }
        public static async Task RawSQLQuery(FootballLeagueDbContext context)
        {
            string name = "AS Roma";
            // Be careful using .FromSqlRaw("") because it opens possibilities for SQL Injection Attacks if you're not being extra careful]
            // Using Literal strings ALWAYS open for sql injection, not being a good practice at all
            var teams1 = await context.Teams.FromSqlRaw($"SELECT * FROM TEAMS WHERE NAME = '{name}'").Include(q => q.Coach).ToListAsync();


            // Recommended - Creates parameters in background in order to mount the query and execute in the db.
            // No longer is necessary to specify singles quotes (') around the name variable, which is another advantage
            // of .FromSqlInterpolated().
            var teams2 = await context.Teams.FromSqlInterpolated($"SELECT * FROM TEAMS WHERE NAME = {name}").ToListAsync();
        }

        public static async Task QueryView(FootballLeagueDbContext context)
        {
            var details = await context.TeamsCoachesLeagues.ToListAsync();

        }

        public static async Task UpdatingTeamRemovingCoach(FootballLeagueDbContext context)
        {
            //var team = await context.Teams.Include(t => t.Coach).FirstAsync(t => t.Id == 4);

            Team team = new Team()
            {
                Id = 4,
                Name = "Juventus Update",
                Coach = new Coach()
                {
                    Id = 4,
                    Name = "Saulo de Melo Macambira Coach"
                }
            };

            if (string.IsNullOrEmpty(team.Coach?.Name))
            {
                context.Coaches.Remove(team.Coach);
            } 
            context.Teams.Attach(team);


            var entriesBeforeSave = context.ChangeTracker.Entries();


            context.Entry(team).Property(t => t.Name).IsModified = true;
            context.Entry(team).Reference(t => t.Coach).IsModified = true;

            await context.SaveChangesAsync();

            var entriesAfterSave = context.ChangeTracker.Entries();

            Console.WriteLine($"Team: {team.Id} - {team.Name};  Coach: {team.Coach?.Id} - {team.Coach?.Name}");
        }


        public static async Task Section27(FootballLeagueDbContext context)
        {
            Console.WriteLine("\n1) Selecting only one property");
            List<string> teams = await context.Teams.Select(t => t.Name).ToListAsync();

            Console.WriteLine("\n2) Selecting multiple properties with Anonymous Type (is called Anonymous Projection)");
            var teams2 = await context.Teams
                                        .Include(t => t.Coach)
                                        .Select(t => new { 
                                            TeamId = t.Id,
                                            TeamName = t.Name,
                                            CoachName = t.Coach.Name
                                        })
                                        .ToListAsync();

            foreach (var team in teams2)
            {
                Console.WriteLine($"TeamId: {team.TeamId}; TeamName: {team.TeamName}; CoachName: {team.CoachName};");
            }

            Console.WriteLine("\n3) Strongly Typed Projection");
            List<TeamDetail> teamDetails = await context.Teams
                                                         .Include(t => t.Coach)
                                                         .Include(t => t.League)
                                                         .Select(t => new TeamDetail
                                                         {
                                                             TeamId = t.Id,
                                                             TeamName = t.Name,
                                                             CoachName = t.Coach.Name,
                                                             LeagueName = t.League.Name
                                                         })
                                                         .AsNoTracking()
                                                         .ToListAsync();



            Console.WriteLine("\n4) Filtering with Related Data");
            var leagues = await context.Leagues.Where(l => l.Teams.Any(t => t.Name.Contains("Bay"))).Include(t => t.Teams).ToListAsync();
        }


        public static async Task AddNewCoach(FootballLeagueDbContext context) { 
            
            var coach = new Coach() { Name = "Jose Mourinho", TeamId = 3};
            context.Coaches.Add(coach);

            var coach2 = new Coach() { Name = "Antonio Conte"};
            context.Coaches.Add(coach2);

            await context.SaveChangesAsync();
        }
        public static async Task AddNewMatches(FootballLeagueDbContext context) {
            var matchs = new List<Match>()
            {
                new Match(){ AwayTeamId = 1, HomeTeamId = 2, Date = new DateTime(2022, 10, 28)},
                new Match(){ AwayTeamId = 8, HomeTeamId = 7, Date = new DateTime(2022, 10, 28)},
                new Match(){ AwayTeamId = 8, HomeTeamId = 7, Date = new DateTime(2022, 10, 28)}

            };
            context.Matches.AddRange(matchs);
            await context.SaveChangesAsync();
        }
        public static async Task AddNewLeagueWithTeams(FootballLeagueDbContext context) {
            League league = new League()
            {
                Name = "CIFA",
                Teams = new List<Team>()
                {
                    new Team() { Name = "Rivoli United" },
                    new Team() { Name = "Waterhouse FC" }
                }
            };

            context.Leagues.Add(league);
            await context.SaveChangesAsync();
        
        }
        public static async Task AddNewTeamWithLeagueId(FootballLeagueDbContext context) {
            Team team = new Team()
            {
                Name = "Bayern Munich",
                LeagueId = 6
            };

            context.Teams.Add(team);
            await context.SaveChangesAsync();
        }
        public static async Task AddNewTeamsWithLeague(FootballLeagueDbContext context) {
            List<Team> teams = new List<Team>()
            {
                new Team(){ Name = "Corithians", League = new League(){ Name = "Libertadores" } },
                new Team(){ Name = "Fortaleza", League = new League(){ Name = "Brasileirão" } }
            };

            context.AddRange(teams);

            await context.SaveChangesAsync();
        
        }


        public static async Task TrackingVsNoTracking(FootballLeagueDbContext context)
        {
            var withTracking = await context.Teams.FirstOrDefaultAsync(t => t.Id == 2);
            var withNoTracking = await context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == 7);

            withTracking.Name = "Inter Milan";
            withNoTracking.Name = "Inter Milan";

            var entriesBeforeSave = context.ChangeTracker.Entries();

            await context.SaveChangesAsync();

            var entriesAfterSave = context.ChangeTracker.Entries();
        }
        public static async Task DeleteWithRelationship(FootballLeagueDbContext context)
        {
            var league = await context.Leagues.FindAsync(5);
            context.Leagues.Remove(league);
            await context.SaveChangesAsync();
        }

        public static async Task SimpleDelete(FootballLeagueDbContext context)
        {
            var league = await context.Leagues.FindAsync(4);
            context.Leagues.Remove(league);
            await context.SaveChangesAsync();
        }

        public static async Task SimpleUpdateTeamRecord(FootballLeagueDbContext context)
        {
            var team = new Team()
            {
                Id = 5, // If Id (PK) is assigned with some value, Entity Framework will automatically pursuit the Team with such Id when trying to update
                Name = "Tivoli Gardens FC Test", // assuming Name has a different value than the original value
                LeagueId = 3 // assuming LeagueId has a different value than the original value
            };

            context.Teams.Update(team);
            await context.SaveChangesAsync("Test Team Management User");
        }

        public static async Task UpdateRecord(FootballLeagueDbContext context)
        {
            int id = 3;
            var league = await context.Leagues.FindAsync(id);

            league.Name = "Scottish Premiership";

            await  context.SaveChangesAsync();

            GetLeagueRecordPrint(id, context);
        }

        private static async void GetLeagueRecordPrint(int id, FootballLeagueDbContext context)
        {
            var league = await context.Leagues.FindAsync(id);

            Console.WriteLine($"{league.Id} - {league.Name}");
        }

        public static async Task AlternativeLinqSyntax(FootballLeagueDbContext context)
        {
            Console.WriteLine("Enter a Team Name (Or Part Of):");
            var teamName = Console.ReadLine();

            //var teams = from i in context.Teams select i;
            //var teams = await (from i in context.Teams select i).ToListAsync();
            var teams = await (from i in context.Teams
                               where EF.Functions.Like(i.Name, $"%{teamName}%")
                               select i).ToListAsync();

            foreach (var t in teams)
            {
                Console.WriteLine($"{t.Id} - {t.Name}");
            }
        }

        public static async Task AdditionalExecutionMethods(FootballLeagueDbContext context)
        {
            //var l = await context.Leagues.Where(q => q.Name.Contains("A")).FirstOrDefaultAsync();
            //var l = await context.Leagues.FirstOrDefaultAsync(q => q.Name.Contains("A"));
            var leagues = context.Leagues;
            var list = await leagues.ToListAsync();
            var first = await leagues.FirstAsync(); // expects the first record to be returned. Otherwise, throw an Error.
            var firstOrDefault = await leagues.FirstOrDefaultAsync(); // expects the first record to be returned.
            //var single = await leagues.SingleAsync(); // expects only one record to be returned. Otherwise, throw an Error.
            var singleOrDefault = await leagues.SingleOrDefaultAsync(); // expects only one record to be returned.

            // Aggregation functions
            //var count = await leagues.CountAsync();
            //var longCount = await leagues.LongCountAsync();
            //var min = await leagues.MinAsync();
            //var max = await leagues.MaxAsync();

            // Dbset Method that will execute
            var league = await leagues.FindAsync(1); // expects to pass the primary key value as parameter of the function (in this case, int id).

        }

        public static async Task QueryFilter(FootballLeagueDbContext context)
        {
            Console.WriteLine("Enter a League Name (Or Part Of):");
            var leagueName = Console.ReadLine();
            var leagues = await context.Leagues.Where(l => l.Name == leagueName).ToListAsync();
            foreach (var league in leagues)
            {
                Console.WriteLine($"{league.Id} - {league.Name}");
            }

            //var partialMatchs = await context.Leagues.Where(l => l.Name.Contains(leagueName)).ToListAsync();
            //var partialMatchs = await context.Leagues.Where(l => EF.Functions.Like(l.Name, "%Bun%")).ToListAsync();
            var partialMatchs = await context.Leagues.Where(l => EF.Functions.Like(l.Name, $"%{leagueName}%")).ToListAsync();
            foreach (var league in partialMatchs)
            {
                Console.WriteLine($"{league.Id} - {league.Name}");
            }
        }

        public static async Task AddTeamsWithLeagueId(League league, FootballLeagueDbContext context)
        {
            var teams = new List<Team>()
            {
                new Team() {
                    Name = "Juventus",
                    LeagueId = league.Id,
                },
                new Team() {
                    Name = "AC Milan",
                    LeagueId = league.Id,
                },
                 // assigning the navigation property value with a new instance, instead of giving value to LeagueId
                new Team() {
                    Name = "AS Roma",
                    League = league,
                },

            };

            context.AddRange(teams);
            await context.SaveChangesAsync();
        }

        public static async Task SimpleSelectQuery(FootballLeagueDbContext context)
        {
            /*  Doing like that, ef will query out in the database all Leagues BUT will only close the connection with the database 
             *  once the for loop has iterated all items from List<League> leagues collection;
                var leagues = await context.Leagues; 

                Always remember: Connection with database is an expensive operation
            */

            var leagues = await context.Leagues.ToListAsync();
            foreach (League league in leagues)
            {
                Console.WriteLine($"{league.Id} - {league.Name}");
            }
        }
    }
}
