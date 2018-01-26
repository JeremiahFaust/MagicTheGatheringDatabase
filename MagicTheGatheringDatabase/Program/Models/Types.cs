using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    [Table("Types")]
    public class Types
    {
        [Key]
        public int ID { get; set; }

        public String Name { get; set; }
        
    }
}
