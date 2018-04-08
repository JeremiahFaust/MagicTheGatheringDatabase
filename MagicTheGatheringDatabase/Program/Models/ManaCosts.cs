using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class ManaCosts
    {
        [MaxLength(20)]
        public string CardID { get; set; }
        [MaxLength(20)]
        public string CardNumber { get; set; }
        public int ColorID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ColorID")]
        public Color Color { get; set; }
        
        public Card Cards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            ManaCosts tmp = (ManaCosts)obj;
            if (tmp.ColorID.Equals(this.ColorID) && tmp.CardID.Equals(this.CardID) && tmp.CardNumber.Equals(this.CardNumber)&& tmp.Quantity.Equals(this.Quantity)) return true;
            return false;
        }
        
    }
}
