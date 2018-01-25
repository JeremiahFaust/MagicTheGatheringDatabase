using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Color
    {
        [Key]
        public int ColorID { get; set; }

        public char ColorSymbol { get; set; }
        public String ColorName { get; set; }

        public int ColorValue { get; set; }
        
        public ICollection<ManaCosts> ManaCosts { get; set; }

    }
}
