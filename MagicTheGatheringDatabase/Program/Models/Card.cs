using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MagicDbContext.Models
{
    [Table("Cards")]
    public class Card
    {
        [Key]
        public int MultiverseID { get; set; }

        [Required]
        public String CardName { get; set; }

        [MaxLength(10)]
        public string SetID { get; set; }
        [ForeignKey("SetID")]
        public Sets Set { get; set; }
        

        public String Rarity { get; set; }


        public int ConvertedManaCost { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        

        public String FlavorText { get; set; }

        public String Artist { get; set; }

        public int Rating { get; set; }
        
        public ICollection<Rulings> Rulings { get; set; }
        public ICollection<CardAbilities> CardAbilities { get; set; }
        public ICollection<ManaCosts> ManaCosts { get; set; }
        public ICollection<CardTypes> CardTypes { get; set; }

        public double LowPrice { get; set; }
        public double MidPrice { get; set; }
        public double HighPrice { get; set; }

        public String ImagePath { get; set; }
    }
}
