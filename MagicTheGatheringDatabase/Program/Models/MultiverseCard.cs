using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class MultiverseCard
    {
        [Key]
        public string MultiverseId { get; set; }

        [MaxLength(10)]
        public string SetID { get; set; }

        [ForeignKey("SetID")]
        public Sets Set { get; set; }

        public String ImagePath { get; set; }
        
        public List<Card> cards { get; set; }

    }
}
