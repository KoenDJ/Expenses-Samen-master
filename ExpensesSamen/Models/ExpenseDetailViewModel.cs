﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesSamen.Models
{
    public class ExpenseDetailViewModel
    {
        public string Omschrijving { get; set; }
        public DateTime Datum { get; set; }
        public decimal Bedrag { get; set; }
        public string Categorie { get; set; }
        public string PhotoUrl { get; set; }
    }
}
