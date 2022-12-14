using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TeamDetail
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string CoachName { get; set; }
        public string LeagueName { get; set; }
    }
}
