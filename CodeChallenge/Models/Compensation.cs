using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key()]
        public virtual Employee Employee { get; set; }
        public Decimal Salary { get; set; }
        [Key()]
        public DateTime EffectiveDate { get; set; }
    }
}
