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

    }
}
