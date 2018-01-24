using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Program.Models
{
    class CardTypes
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Type")]
        public int TypeID { get; set; }
        public Types Type { get; set; }

        [ForeignKey("Card")]
        public int CardID { get; set; }
        public Cards Card { get; set; }
    }
}
