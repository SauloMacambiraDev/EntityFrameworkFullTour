using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Team> TeamRepository { get; }
        IRepository<League> LeagueRepository { get; }
        IRepository<Coach> CoachRepository { get; }
        IRepository<Match> MatchRepository { get; }
        void Commit();
        Task CommitAsync();
        Task CommitAsync(string user);
    }
}
