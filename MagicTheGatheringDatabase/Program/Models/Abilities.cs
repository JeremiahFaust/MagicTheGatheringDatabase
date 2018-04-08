using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Abilities
    {
        [Key]
        public int AbilityID { get; set; }

        public String Ability { get; set; }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            Abilities a = (Abilities)obj;

            if (a.AbilityID.Equals(this.AbilityID) && a.Ability.Equals(this.Ability)) return true;

            return false;    
        }

    }
}
