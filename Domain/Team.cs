using Domain.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public  class Team: BaseDomainObject
    {
        public string Name { get; set; }
        public int LeagueId { get; set; }
        public virtual League League { get; set; }
        public Coach Coach { get; set; }

        public List<Match> HomeMatches { get; set; }
        public List<Match> AwayMatches { get; set; }
    }
}
