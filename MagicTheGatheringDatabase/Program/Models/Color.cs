using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Color
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }


    }
}
