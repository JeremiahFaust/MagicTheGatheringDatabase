using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Color
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            Color c = (Color)obj;

            if (c.ID.Equals(this.ID) && c.Name.Equals(this.Name) && c.Symbol.Equals(this.Symbol)) return true;
            return false;
        }
    }
}
