using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Sets
    {
        [Key]
        [MaxLength(10)]
        public String SetAbbr { get; set; }

        public string SetFullName { get; set; }

        public ICollection<Card> Cards { get; set; }

    }
}
