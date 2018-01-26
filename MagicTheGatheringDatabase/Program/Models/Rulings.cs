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

        [ForeignKey("Cards")]
        public int CardID { get; set; }
        public Card Cards { get; set; }
    }
}
