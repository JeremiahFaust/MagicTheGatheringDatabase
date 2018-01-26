using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class ManaCosts
    {
        public int CardID { get; set; }
        public int ColorID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ColorID")]
        public Color Color { get; set; }

        [ForeignKey("CardID")]
        public Card Cards { get; set; }
    }
}
