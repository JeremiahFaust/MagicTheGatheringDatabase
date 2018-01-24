using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Program.Models
{
    class Sets
    {
        [Key]
        public int ID { get; set; }

        public String SetAbbr { get; set; }

        public string SetFullName { get; set; }

        public ICollection<Cards> Cards { get; set; }

    }
}
