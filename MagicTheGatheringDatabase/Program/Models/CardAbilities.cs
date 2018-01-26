using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class CardAbilities
    {
        [ForeignKey("Ability")]
        public int AbilityID { get; set; }
        public Abilities Ability { get; set; }

        [ForeignKey("Card")]
        public string CardID { get; set; }
        public Card Card { get; set; }
    }
}