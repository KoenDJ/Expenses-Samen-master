using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpensesSamen.Database;
using ExpensesSamen.Domain;
using ExpensesSamen.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesSamen.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseDatabase _expenseDatabase;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExpenseController(IExpenseDatabase expenseDatabase, IWebHostEnvironment webHostEnvironment)
        {
            _expenseDatabase = expenseDatabase;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var expenses = _expenseDatabase.GetExpenses().Select(x => new ExpenseIndexViewModel
            {
                Bedrag = x.Bedrag,
                Categorie = x.Categorie,
                Datum = x.Datum,
                Id = x.Id,
                Omschrijving = x.Omschrijving
            });

            return View(expenses);
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            var expense = _expenseDatabase.GetExpense(id);
            return View(new ExpenseEditViewModel
            {
                Bedrag = expense.Bedrag,
                Categorie = expense.Categorie,
                Datum = expense.Datum,
                Omschrijving = expense.Omschrijving,
                PhotoUrl = expense.PhotoUrl,
                Id = expense.Id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] ExpenseEditViewModel expense, [FromRoute] int id)
        {
            if (TryValidateModel(expense))
            {
                _expenseDatabase.Update(id, new Expense
                {
                    Bedrag = expense.Bedrag,
                    Categorie = expense.Categorie,
                    Datum = expense.Datum,
                    Omschrijving = expense.Omschrijving,
                    PhotoUrl = expense.PhotoUrl
                });
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(expense);
            }
        }

        public IActionResult Detail([FromRoute] int id)
        {
            var expense = _expenseDatabase.GetExpense(id);
            return View(new ExpenseDetailViewModel
            {
                Bedrag = expense.Bedrag,
                Categorie = expense.Categorie,
                Datum = expense.Datum,
                Omschrijving = expense.Omschrijving,
                PhotoUrl = expense.PhotoUrl
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ExpenseCreateViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ExpenseCreateViewModel expense)
        {
            if (!TryValidateModel(expense))
            {
                return View(expense);
            }

            Expense newExpense = new Expense
            {
                Bedrag = expense.Bedrag,
                Categorie = expense.Categorie,
                Datum = expense.Datum,
                Omschrijving = expense.Omschrijving
            };

            if (expense.Photo != null)
            {
                string uniqueFileName = UploadPhoto(expense.Photo);
                newExpense.PhotoUrl = "/photos/" + uniqueFileName;
            }

            _expenseDatabase.Insert(newExpense);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            var expense = _expenseDatabase.GetExpense(id);

            return View(new ExpenseDeleteViewModel
            {
                Id = id,
                Omschrijving = expense.Omschrijving
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromForm] ExpenseDeleteViewModel expense, [FromRoute] int id)
        {
            if (TryValidateModel(expense))
            {
                _expenseDatabase.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(expense);
            }
        }

        private string UploadPhoto(IFormFile photo)
        {
                                    //uniek ID                  .jpg
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                                           //Pad naar wwwroot               /photos
            string pathName = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
                                                  //wwwroot/photos/id.jpg
            string fileNameWithPath = Path.Combine(pathName, uniqueFileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return uniqueFileName;
        }
    }
}
