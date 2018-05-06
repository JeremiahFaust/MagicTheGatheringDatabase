using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class CardSets
    {
        [Key]
        [MaxLength(255)]
        public string Name { get; set; }

        public string MultiverseId { get; set; }

        [ForeignKey("MultiverseId")]
        public MultiverseCard DisplayCard { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}
