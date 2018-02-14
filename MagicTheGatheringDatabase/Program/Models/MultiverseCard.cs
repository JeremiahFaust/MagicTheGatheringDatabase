using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class MultiverseCard
    {
        [Key]
        public string MultiverseId { get; set; }

        [MaxLength(10)]
        public string SetID { get; set; }

        [ForeignKey("SetID")]
        public Sets Set { get; set; }

        public String ImagePath { get; set; }
        
        public List<Card> cards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            MultiverseCard mc = (MultiverseCard)obj;

            if (mc.MultiverseId.Equals(this.MultiverseId) && mc.SetID.Equals(this.SetID) && mc.ImagePath.Equals(this.ImagePath)) return true;


            return false;
        }

    }
}
