using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Program.Models
{
    class Color
    {
        [Key]
        public int ID { get; set; }

        public char ColorSymbol { get; set; }
        public String ColorName { get; set; }

        public int ColorValue { get; set; }
        
        public ICollection<ManaCosts> ManaCosts { get; set; }

    }
}
