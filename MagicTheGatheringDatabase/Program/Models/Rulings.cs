using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class Rulings
    {
        public String Ruling { get; set; }

        public DateTime Date { get; set; }
        
        public string CardID { get; set; }
        public string CardNumber { get; set; }
        public Card Cards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            Rulings r = (Rulings)obj;

            if (r.Ruling.Equals(this.Ruling) && r.CardID.Equals(this.CardID) && r.CardNumber.Equals(this.CardNumber) && r.Date.Equals(this.Date)) return true;

            return base.Equals(obj);
        }
    }
}
