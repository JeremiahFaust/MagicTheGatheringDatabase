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

        [ForeignKey("Card")]
        public string CardID { get; set; }
        public Card Card { get; set; }
    }
}
