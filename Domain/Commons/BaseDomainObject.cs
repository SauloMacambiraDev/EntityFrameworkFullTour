using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons
{
    public abstract class BaseDomainObject
    {
        public int Id { get; set; }


        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
