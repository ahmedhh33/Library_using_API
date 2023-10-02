using Library_web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_web.Controllers
{
    [Route("Book/BookManagement")]
    [ApiController]
    public class BookManagementController : ControllerBase
    {
        public static ApplicationDbContext _context;

        public BookManagementController(ApplicationDbContext DB)
        {
            _context = DB;
        }
        [HttpPost]
        public IActionResult AddBook(string title, string author, int publication_year)
        {
            try
            {
                BookManagement bookManagement = new BookManagement
                {
                    title = title,
                    author = author,
                    publication_year = publication_year,
                    is_Available = true,
                };

                _context.bookManagements.Add(bookManagement);
                _context.SaveChanges();

                return Ok("Book added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Error adding book: " + ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Remove(int ID)
        {
            try
            {
                BookManagement bookManagement = _context.bookManagements.SingleOrDefault(x => x.B_ID == ID);

                if (bookManagement != null)
                {
                    _context.bookManagements.Remove(bookManagement);
                    _context.SaveChanges();

                    return Ok("Book removed successfully");
                }
                else
                {
                    return NotFound("Book not found");
                }
            }
            catch (Exception ex)
            {
                
                return BadRequest("Error removing book: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateBook(int ID, string title, string author, int publication_year)
        {
            try
            {
                BookManagement bookManagement = _context.bookManagements.SingleOrDefault(x => x.B_ID == ID);

                if (bookManagement != null)
                {
                    bookManagement.title = title;
                    bookManagement.author = author;
                    bookManagement.publication_year = publication_year;

                    _context.Update(bookManagement);
                    _context.SaveChanges();

                    return Ok("Book updated successfully");
                }
                else
                {
                    return NotFound("Book not found");
                }
            }
            catch (Exception ex)
            {
                
                return BadRequest("Error updating book: " + ex.Message);
            }
        }


        [HttpGet("GetBookByTitle")]
        public  IActionResult GetBookByTitle(string title)
        {
            BookManagement bookManagement = _context.bookManagements.SingleOrDefault(b => b.title == title);
            if (bookManagement != null)
            {
                return Ok(bookManagement);
            }
            else
            {
                return NotFound("No book founs");
            }
            
        }
        [HttpGet("GetBookByAuthor")]
        public  IActionResult GetBookByAuthor(string Author)
        {
            List<BookManagement> bookManagement = _context.bookManagements.Where(b => b.author == Author).ToList();
            if (bookManagement != null)
            {
                return Ok(bookManagement);
            }
            else
            {
                return NotFound("No book founs");
            }

        }
        [HttpGet("GetBookByPublesheryear")]
        public  IActionResult GetBookByPublisherDate(int Date)
        {
            List<BookManagement> bookManagement = _context.bookManagements.Where(b => b.publication_year == Date).ToList();
            if (bookManagement != null)
            {
                return Ok(bookManagement);
            }
            else 
            { 
                return NotFound("No book found");
            }

        }

        [HttpGet("GetBookCountByear")]
        public IActionResult GetBookCount(int PY)
        {
            var bookManagement = _context.bookManagements.Where(b => b.publication_year == PY).Count();
            if (bookManagement>0)
            {
                return Ok(bookManagement);
            }
            else
            {
                return BadRequest("No book published in this year");
            }
        }
        [HttpGet("GetAllBook")]
        public IActionResult GetAllBooks()
        {
            var bookManagements = _context.bookManagements.ToList();
            if(bookManagements!=null) { return Ok(bookManagements); } else { return NotFound("There is no book"); }
        }

        [HttpGet("GetAvailableBook")]
        public IActionResult GetAvailableBooks()
        {
            var bookManagements = _context.bookManagements.Where(x=>x.is_Available==true).ToList();
            if (bookManagements != null) { return Ok(bookManagements); } else { return NotFound("There is no book Available"); }

        }
    }
}
