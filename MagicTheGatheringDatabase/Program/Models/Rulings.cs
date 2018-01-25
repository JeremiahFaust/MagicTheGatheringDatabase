using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class Rulings
    {
        [Key]
        public int ID { get; set; }

        public String Ruling { get; set; }

        [ForeignKey("Cards")]
        public int CardID { get; set; }
        public Cards Cards { get; set; }
    }
}
