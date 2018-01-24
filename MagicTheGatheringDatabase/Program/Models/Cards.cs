using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Program.Models
{
    class Cards
    {
        [Key]
        public int MultiverseID { get; set; }

        [Required]
        public String CardName { get; set; }

        [Required]
        public int SetID { get; set; }
        [Required]
        public Sets Set { get; set; }

        public ICollection<Types> Types { get; set; }

        public String Rarity { get; set; }

        public ICollection<ManaCosts> ManaCosts { get; set; }
        public int ConvertedManaCost { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }

        public ICollection<Abilities> Abilities { get; set; }

        public String FlavorText { get; set; }

        public String Artist { get; set; }

        public int Rating { get; set; }

        public ICollection<Rulings> Rulings { get; set; }

        public ICollection<Color> Colors { get; set; }

        public double LowPrice { get; set; }
        public double MidPrice { get; set; }
        public double HighPrice { get; set; }
    }
}
