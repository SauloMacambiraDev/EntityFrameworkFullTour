using System;
using System.Collections.Generic;

namespace DbFirstTestConsoleApp.Models
{
    public partial class Team
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int LeagueId { get; set; }

        public virtual League League { get; set; } = null!;
    }
}
