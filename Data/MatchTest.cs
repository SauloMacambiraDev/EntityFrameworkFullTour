using Data.Configurations.Entities;
using Domain;
using Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    [EntityTypeConfiguration(typeof(TeamConfiguration))]
    public  class MatchTest: BaseDomainObject
    {
        public int HomeTeamId { get; set; } // Not following EF naming convention
        public virtual Team HomeTeam { get; set; }
        public int AwayTeamId { get; set; } // Not following EF naming convention
        public virtual Team AwayTeam { get; set; }

        [Precision(18,2)]
        public decimal TicketPrice { get; set; }

        public DateTime Date { get; set; }
        
    }
}
