using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class ManaCosts
    {
        [Key]
        public int ManaCostID { get; set; }

        [ForeignKey("Color")]
        public int ColorID { get; set; }
        public Color Color { get; set; }

        [ForeignKey("Card")]
        public int CardID { get; set; }
        public Cards Card { get; set; }
    }
}
