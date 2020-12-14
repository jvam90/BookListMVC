using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDBContext _db;
        [BindProperty]
        public Book Book { get; set; }
        //addcomentario
        public BooksController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if(id == null)
            {
                return View(Book);
            }
            else
            {
                Book = _db.Books.FirstOrDefault(u => u.Id == id);
                if(Book == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(Book);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(Book);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (book == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Delete successful" });
            }

        }
    }
}
