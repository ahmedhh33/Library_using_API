using Library_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingTransactionsController : ControllerBase
    {
        public static ApplicationDbContext _context;

        public BorrowingTransactionsController(ApplicationDbContext DB)
        {
            _context = DB;
        }
        [HttpPost]
        public void CreateBorrowingTransaction(int patronId, int bookId)
        {
            PatronManagement patron = _context.patronManagements.FirstOrDefault(p => p.Pat_ID == patronId);
            BookManagement book = _context.bookManagements.FirstOrDefault(b => b.B_ID == bookId);

            if (patron == null || book == null || !book.is_Available)
            {
                Console.WriteLine("Patron or Book not found");
                return;
            }

            BorrowingTransactions transaction = new BorrowingTransactions
            {
                Pat_ID = patronId,
                B_ID = bookId,
                borrowing_date = DateTime.Now,
                return_date = null // after two weeks
            };

            _context.BorrowingTransactions.Add(transaction);
            book.is_Available = false; // Mark the book as unavailable.
            _context.SaveChanges();
        }
        [HttpPut]
        public void MarkBookAsReturned(int transactionId)
        {
            var transaction = _context.BorrowingTransactions.FirstOrDefault(t => t.TraID == transactionId);

            if (transaction == null || transaction.return_date != null)
            {
                Console.WriteLine(" transaction not found or book is already returned.");
                return;
            }

            transaction.return_date = DateTime.Now;
            var book = _context.bookManagements.FirstOrDefault(b => b.B_ID == transaction.B_ID);

            if (book != null)
            {
                book.is_Available = true; // Mark the book as available.
            }

            _context.SaveChanges();
        }


        [HttpGet]
        public List<BorrowingTransactions> GetTransactionHistoryForPatron(int patronId)
        {
            return _context.BorrowingTransactions
                .Where(t => t.Pat_ID == patronId)
                .Include(t => t.B_ID) // Include the related book information.
                .ToList();
        }
        [HttpGet("GetTransactionHistoryForBook")]
        public List<BorrowingTransactions> GetTransactionHistoryForBook(int bookid)
        {
            return _context.BorrowingTransactions
                .Where(t => t.B_ID == bookid)
                .Include(t => t.Pat_ID) // Include the related patron information.
                .ToList();
        }
    }
}
