using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MagicDbContext.Models
{
    [Table("Cards")]
    public class Card
    {
        [MaxLength(20)]
        public string MultiverseID { get; set; }

        public MultiverseCard MultiverseCard { get; set; }

        [MaxLength(20)]
        public string CardNumber { get; set; }

        [Required]
        public String CardName { get; set; }

        /*
        [MaxLength(10)]
        public string SetID { get; set; }
        [ForeignKey("SetID")]
        public Sets Set { get; set; }
        */
        
        [MaxLength(1)]
        public string RarityId { get; set; }


        public int ConvertedManaCost { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        

        public String FlavorText { get; set; }

        public String Artist { get; set; }

        public int Rating { get; set; }

        [ForeignKey("RarityId")]
        public CardRarity Rarity { get; set; }
        
        public ICollection<Rulings> Rulings { get; set; }
        public ICollection<CardAbilities> CardAbilities { get; set; }
        public ICollection<ManaCosts> ManaCosts { get; set; }
        public ICollection<CardTypes> CardTypes { get; set; }

        public double LowPrice { get; set; }
        public double MidPrice { get; set; }
        public double HighPrice { get; set; }

        //public String ImagePath { get; set; }
        public bool IsDualCard { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            Card c = (Card)obj;
            if (this.MultiverseID == c.MultiverseID && this.CardNumber == c.CardNumber) return true;
            //if (c.MultiverseID.Equals(this.MultiverseID) && c.CardNumber.Equals(this.CardNumber) && c.CardName.Equals(this.CardName) && c.ConvertedManaCost.Equals(this.ConvertedManaCost) && c.FlavorText.Equals(this.FlavorText) && c.Rarity.Equals(this.Rarity) && c.Power.Equals(this.Power) && c.Toughness.Equals(this.Toughness) && c.Rating.Equals(this.Rating) && c.MidPrice.Equals(this.MidPrice) && c.LowPrice.Equals(this.LowPrice) && c.HighPrice.Equals(this.HighPrice) && c.Artist.Equals(this.Artist) && c.IsDualCard.Equals(this.IsDualCard)) return true;
            return false;
        }

        public override string ToString()
        {
            return MultiverseID + " " + CardNumber + ": " + CardName;
        }
    }
}
