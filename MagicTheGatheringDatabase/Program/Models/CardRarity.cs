using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class CardRarity
    {
        [Key]
        [MaxLength(1)]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}
