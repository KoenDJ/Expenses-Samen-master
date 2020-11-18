using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesSamen.Models
{
    public class ExpenseCreateViewModel
    {
        public IEnumerable<SelectListItem> MijnList = new List<SelectListItem> { new SelectListItem("Groceries", "Groceries"), new SelectListItem("Gas", "Gas"), new SelectListItem("Water","Water") };

        [Required(ErrorMessage = "Wilde uw omschrijving invullen")]
        [DisplayName("Omschrijving")]
        public string Omschrijving { get; set; }
        [Required]
        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
        [Required]
        [DisplayName("Bedrag")]
        [DataType(DataType.Currency)]
        public decimal Bedrag { get; set; }
        [Required]
        [DisplayName("Categorie")]
        public string Categorie { get; set; }
        [DisplayName("Foto")]
        public IFormFile Photo { get; set; }
    }
}
