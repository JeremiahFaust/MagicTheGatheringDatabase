using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MagicDbContext.Models
{
    public class Cards
    {
        [Key]
        public int MultiverseID { get; set; }

        [Required]
        public String CardName { get; set; }

        [ForeignKey("Sets")]
        public int SetID { get; set; }
        public Sets Set { get; set; }
        

        public String Rarity { get; set; }


        public int ConvertedManaCost                              
        {
            get
            {
                int sum = 0;
                foreach (ManaCosts c in ManaCosts)
                {
                    sum += c.Color.ColorValue;
                }
                return sum;
            }
        }
        public int Power { get; set; }
        public int Toughness { get; set; }
        

        public String FlavorText { get; set; }

        public String Artist { get; set; }

        public int Rating { get; set; }
        
        public ICollection<Rulings> Rulings { get; set; }
        public ICollection<CardAbilities> CardAbilities { get; set; }
        public ICollection<ManaCosts> ManaCosts { get; set; }
        public String CardType { get; set; }

        public double LowPrice { get; set; }
        public double MidPrice { get; set; }
        public double HighPrice { get; set; }
    }
}
