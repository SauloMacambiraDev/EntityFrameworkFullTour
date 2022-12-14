using Data.Repository.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly FootballLeagueDbContext _context;

        public TeamRepository(FootballLeagueDbContext context)
        {
            _context = context;
        }

        public void Add(Team entity)
        {
            _context.Teams.Add(entity);
        }

        public void Delete(Team entity)
        {
            _context.Teams.Remove(entity);
        }

        public async Task<IEnumerable<Team>> Get()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<IEnumerable<Team>> Get(Expression<Func<Team, bool>> predicate)
        {
            return await _context.Teams.Where(predicate).ToListAsync();
        }

        public async Task<Team> GetById(Expression<Func<Team, bool>> predicate)
        {
            return await _context.Teams.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Team>> GetTeamsOrderedByName()
        {
            return await _context.Teams.OrderBy(t => t.Name).ToListAsync();
        }

        public void Update(Team entity)
        {
            _context.Teams.Update(entity);
        }
    }
}
