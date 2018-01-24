﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Program.Models
{
    class Abilities
    {
        [Key]
        public int ID { get; set; }

        public String Ability { get; set; }

        public ICollection<CardAbilities> CardAbilities { get; set; }

    }
}
