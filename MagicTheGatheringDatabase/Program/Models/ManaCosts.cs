using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Program.Models
{
    class ManaCosts
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Color")]
        public int ColorID { get; set; }
        public Color Color { get; set; }

        public ICollection<Cards> Cards { get; set; }
    }
}
