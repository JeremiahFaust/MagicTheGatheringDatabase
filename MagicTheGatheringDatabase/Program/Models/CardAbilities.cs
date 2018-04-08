using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class CardAbilities
    {
        [ForeignKey("Ability")]
        public int AbilityID { get; set; }
        public Abilities Ability { get; set; }

        [MaxLength(20)]
        public string CardID { get; set; }
        [MaxLength(20)]
        public string CardNumber { get; set; }

        public Card Card { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            CardAbilities ca = (CardAbilities)obj;

            if (ca.AbilityID.Equals(this.AbilityID) && ca.CardID.Equals(this.CardID) && ca.CardNumber.Equals(this.CardNumber)) return true;

            return false;
        }
    }
}