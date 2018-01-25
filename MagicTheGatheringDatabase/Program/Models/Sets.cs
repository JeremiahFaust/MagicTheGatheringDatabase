using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagicDbContext.Models
{
    public class Sets
    {
        [Key]
        public int SetID { get; set; }

        public String SetAbbr { get; set; }

        public string SetFullName { get; set; }

        public ICollection<Cards> Cards { get; set; }

    }
}
