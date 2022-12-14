// See https://aka.ms/new-console-template for more information
using ConsoleApp;
using Data;
using Domain;
using Microsoft.EntityFrameworkCore;

try
{
    Console.WriteLine("Hello, World!");
    FootballLeagueDbContext context = new FootballLeagueDbContext();

    //League league = new League() { Id = 3, Name = "Bundesligaa" };
    //Team team = new Team() { Name = "Bayern Munich", League = league };
    //context.Add(team);

    //Task.WaitAll(context.SaveChangesAsync());
    //Task.WaitAll(Utility.AddTeamsWithLeagueId(league, context));

    //Utility.SimpleSelectQuery(context).Wait();
    //Task.WaitAll(Utility.QueryFilter(context));
    //Utility.AlternativeLinqSyntax(context).Wait();
    //Utility.UpdateRecord(context).Wait();
    //Utility.SimpleUpdateTeamRecord(context).Wait();
    //Utility.SimpleDelete(context).Wait();
    //Utility.DeleteWithRelationship(context).Wait();

    //Console.WriteLine($"\nLeague with id: {league.Id}; Name: {league.Name}; was created successfully!");

    //Utility.AddNewTeamsWithLeague(context).Wait();
    //Utility.AddNewTeamWithLeagueId(context).Wait();
    //Utility.AddNewLeagueWithTeams(context).Wait();
    //Utility.AddNewCoach(context).Wait();
    //Utility.Section27(context).Wait();
    //Utility.UpdatingTeamRemovingCoach(context).Wait();
    //Utility.QueryView(context).Wait();
    //Utility.RawSQLQuery(context).Wait();    

    //Utility.SimpleUpdateTeamRecord(context).Wait();
    //Utility.ExecutingDefaultTransaction(context).Wait();
    Utility.ExecutingTransactionWithSavePoint(context).Wait();

}
catch (Exception ex)
{
    Console.WriteLine("An error ocurred:");
    Console.WriteLine(ex.Message);
} finally
{
    Console.WriteLine("\nType anything to finish the execution of the console app..");
    Console.ReadLine();
}
