using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Sets
    {
        [Key]
        [MaxLength(10)]
        public String SetAbbr { get; set; }

        public string SetFullName { get; set; }

        //public ICollection<Card> Cards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            Sets s = (Sets)obj;

            if (s.SetAbbr.Equals(this.SetAbbr) && s.SetFullName.Equals(this.SetFullName)) return true;

            return false;
        }

        public override string ToString()
        {
            return SetAbbr + ": " + SetFullName;
        }
    }
}
