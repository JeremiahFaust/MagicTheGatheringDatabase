using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class CardTypes
    { 
        [ForeignKey("Type")]
        public int TypeID { get; set; }
        public Types Type { get; set; }
        
        [MaxLength(20)]
        public string CardID { get; set; }
        [MaxLength(20)]
        public string CardNumber { get; set; }
        public Card Card { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            CardTypes ct = (CardTypes)obj;

            if (ct.CardID.Equals(this.CardID) && ct.CardNumber.Equals(this.CardNumber) && ct.TypeID.Equals(this.TypeID)) return true;

            return false;
        }
    }
}
