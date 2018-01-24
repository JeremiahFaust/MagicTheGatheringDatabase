using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Program.Models
{
    class CardAbilities
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Ability")]
        public int AbilityID { get; set; }
        public Abilities Ability { get; set; }

        [ForeignKey("Card")]
        public int CardID { get; set; }
        public Cards Card { get; set; }

    }
}
