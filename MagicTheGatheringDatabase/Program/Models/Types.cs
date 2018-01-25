using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Types
    {
        [Key]
        public int ID { get; set; }

        public String TypeName { get; set; }

        public ICollection<Cards> Cards { get; set; }
    }
}
