using Data.Repository.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FootballLeagueDbContext _context;
        private Repository<Team> _teamRepository;
        private Repository<League> _leagueRepository;
        private Repository<Coach> _coachRepository;
        private Repository<Match> _matchRepository;

        public UnitOfWork(FootballLeagueDbContext context)
        {
            _context = context;
        }

        public UnitOfWork()
        {
            _context = new FootballLeagueDbContext();
        }


        public IRepository<Team> TeamRepository { 
            get
            {
                return _teamRepository ?? new Repository<Team>(_context);
            } 
        }

        public IRepository<League> LeagueRepository
        {
            get
            {
                return _leagueRepository ?? new Repository<League>(_context);
            }
        }

        public IRepository<Coach> CoachRepository
        {
            get
            {
                return _coachRepository ?? new Repository<Coach>(_context);
            }
        }

        public IRepository<Match> MatchRepository
        {
            get
            {
                return _matchRepository ?? new Repository<Match>(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CommitAsync(string user)
        {
            await _context.SaveChangesAsync(user);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
