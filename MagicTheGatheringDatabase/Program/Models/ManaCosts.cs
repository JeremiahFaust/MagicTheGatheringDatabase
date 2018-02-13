using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class ManaCosts
    {
        public string CardID { get; set; }
        public string CardNumber { get; set; }
        public int ColorID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ColorID")]
        public Color Color { get; set; }
        
        public Card Cards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ManaCosts)) return false;
            ManaCosts tmp = (ManaCosts)obj;
            if (tmp.ColorID == this.ColorID && tmp.CardID.Equals(this.CardID)) return true;
            return false;
        }
    }
}
