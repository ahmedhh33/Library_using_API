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
        public  void AddBook(BookManagement Book)
        {
            _context.bookManagements.Add(Book);
            _context.SaveChanges();
        }
        [HttpDelete]
        public  void Remove(int ID)
        {
            BookManagement bookManagement = _context.bookManagements.SingleOrDefault(x => x.B_ID == ID);

            if (bookManagement != null)
            {
                _context.bookManagements.Remove(bookManagement);
                _context.SaveChanges();

                Console.WriteLine("Book Removes SUccessfully");
            }
            else
            {
                Console.WriteLine("Book Not Found");
            }
        }
        [HttpPut]
        public  void UpdateBook(int ID, string title, string author, int publication_year)
        {
            BookManagement bookManagement = _context.bookManagements.SingleOrDefault(x => x.B_ID == ID);

            if (bookManagement != null)
            {
                bookManagement.title = title;
                bookManagement.author = author;
                bookManagement.publication_year = publication_year;
                _context.Update(bookManagement);
                _context.SaveChanges();

                Console.WriteLine("Book updated SUccessfully");
            }
            else
            {
                Console.WriteLine("Book Not Found");
            }
        }

        [HttpGet]
        public  BookManagement GetBookByTitle(string title)
        {
            BookManagement bookManagement = _context.bookManagements.SingleOrDefault(b => b.title == title);
            return bookManagement;
        }
        [HttpGet("GetBookByAuthor")]
        public  List<BookManagement> GetBookByAuthor(string Author)
        {
            List<BookManagement> bookManagement = _context.bookManagements.Where(b => b.author == Author).ToList();
            return bookManagement;

        }
        [HttpGet("GetBookByPublesheryear")]
        public  List<BookManagement> GetBookByPublisherDate(int Date)
        {
            List<BookManagement> bookManagement = _context.bookManagements.Where(b => b.publication_year == Date).ToList();
            return bookManagement;

        }
    }
}
