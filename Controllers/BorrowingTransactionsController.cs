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
        [HttpDelete]
        public void Remove(int transactionId)
        {
            BorrowingTransactions borrowingTransactions = _context.BorrowingTransactions.FirstOrDefault(x=>x.TraID== transactionId);
            if (borrowingTransactions != null)
            {
                _context.BorrowingTransactions.Remove(borrowingTransactions);
                _context.SaveChanges();
                Console.WriteLine("BoorrowingTra Removes SUccessfully");
            }
            else
            {
                Console.WriteLine("Tra Not Found");
            }
        }

        [HttpGet("GetTransactionHistoryForPatron")]
        public void GetTransactionHistoryForPatron(int patronId)
        {
            var ph = _context.patronManagements.Include(x => x.borrowingTransactions)
                .ThenInclude(x => x.BookManagement).FirstOrDefault(x=>x.Pat_ID==patronId);
            if (ph != null)
            {
                var BH = ph.borrowingTransactions.OrderByDescending(B=>B.borrowing_date).ToList();
                foreach (var transaction in BH)
                {
                    Console.WriteLine($"TraID: {transaction.TraID}\n BookID: {transaction.B_ID}\n BookTitle: {transaction.BookManagement.title}\n BorrowDate: {transaction.borrowing_date}\n Returndate: {transaction.return_date}\n -------------------");
                }
                //return _context.BorrowingTransactions
                //    .Include(t => t.PatronManagement)// Include the book information.
                //    .Include(x => x.BookManagement)
                //    .FirstOrDefault(t => t.Pat_ID == patronId).ToList();
            }
            else
            {
                Console.WriteLine("No Data");
            }
        }
        [HttpGet("GetTransactionHistoryForBook")]
        public void GetTransactionHistoryForBook(int bookid)
        {
            var Bh = _context.bookManagements.Include(x => x.borrowingTransactions)
                .ThenInclude(x => x.PatronManagement).FirstOrDefault(x => x.B_ID == bookid);
            if (Bh != null)
            {
                var BH = Bh.borrowingTransactions.OrderByDescending(B => B.borrowing_date).ToList();
                foreach (var transaction in BH)
                {
                    Console.WriteLine($"TraID: {transaction.TraID}\n PatronID: {transaction.Pat_ID}\n PatronName: {transaction.PatronManagement.Name}\n BorrowDate: {transaction.borrowing_date}\n Returndate: {transaction.return_date}\n -------------------");
                }
                
            }
            else
            {
                Console.WriteLine("No Data");
            }

            //return _context.BorrowingTransactions
            //    .Where(t => t.B_ID == bookid)
            //    .Include(t => t.Pat_ID) // Include the related patron information.
            //    .ToList();
        }
        //[HttpGet("AllTransaction")]
        [HttpGet("AllTransaction")]
        public IActionResult GetAllTransactionHistory()
        {
            var transactions = _context.BorrowingTransactions
                .Include(t => t.BookManagement)
                .Include(t => t.PatronManagement)
                .OrderByDescending(B => B.borrowing_date)
                .ToList();

            // Check if transactions were found
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound(); 
            }

            return Ok(transactions); 
        }

        //public List<BorrowingTransactions> GetAllTransactionHistory()
        //{

        //        var transactions = _context.BorrowingTransactions
        //            .Include(t => t.BookManagement)
        //            .Include(t => t.PatronManagement)
        //            .OrderByDescending(B => B.borrowing_date).ToList();

        //    return transactions;
        //    //foreach (var transaction in BRH)
        //    //{
        //    //    Console.WriteLine($"TraID: {transaction.TraID}\n BookID: {transaction.B_ID}\n BookName: {transaction.BookManagement.title}\n PatronID: {transaction.Pat_ID}\n PatronName: {transaction.PatronManagement.Name}\n BorrowDate: {transaction.borrowing_date}\n Returndate: {transaction.return_date}\n -------------------");
        //    //}
        //}
        //public List<BorrowingTransactions> GetAllBorrowingsInDate(DateOnly date)
        //{
        //    var transaction = _context.BorrowingTransactions.Include(t=>t.BookManagement)
        //                                                    .Include(t=> t.PatronManagement)
        //                                                    .Where(r=>r.borrowing_date == date).ToList();
        //    return transaction;
        //}
    }
}
