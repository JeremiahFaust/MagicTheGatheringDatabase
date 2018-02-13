﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagicDbContext.Models
{
    public class Rulings
    {
        public String Ruling { get; set; }

        public DateTime Date { get; set; }
        
        public string CardID { get; set; }
        public string CardNumber { get; set; }
        public Card Cards { get; set; }
    }
}
