using Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Coach : BaseDomainObject
    {
        public string Name { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
    }
}
