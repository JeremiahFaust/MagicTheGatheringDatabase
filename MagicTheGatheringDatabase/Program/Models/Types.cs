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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            Types t = (Types)obj;

            if (t.ID.Equals(this.ID) && t.Name.Equals(this.Name)) return true;

            return false;
        }

    }
}
