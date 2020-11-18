using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesSamen.Models
{
    public class ExpenseDto
    {
        public int ID { get; set; }
        public string  Description { get; set; }
        public decimal Value { get; set; }
    }
}
