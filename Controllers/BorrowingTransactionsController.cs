using Library_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingTransactionsController : ControllerBase
    {
        HttpClient client;
        public static ApplicationDbContext _context;

        public BorrowingTransactionsController(ApplicationDbContext DB)
        {
            client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7088/");
            _context = DB;
        }
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBorrowingTransaction(int patronId, int bookId,int AccountNumber,int AccountHolderID)
        {
            try
            {
                PatronManagement patron = _context.patronManagements.FirstOrDefault(p => p.Pat_ID == patronId);
                BookManagement book = _context.bookManagements.FirstOrDefault(b => b.B_ID == bookId);

                if (patron == null || book == null || !book.is_Available)
                {
                    return BadRequest("Patron or Book not found or the book is not available");
                }

                BorrowingTransactions transaction = new BorrowingTransactions
                {
                    Pat_ID = patronId,
                    B_ID = bookId,
                    borrowing_date = DateTime.Now,
                    return_date = null
                };

                var withdrawRequest = new WithdrawRequest
                {
                    AccountNumber = AccountNumber,
                    Amount = 3,
                    AccountHolderID = AccountHolderID
                };

                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7088/");

                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Transaction/Withdraw", withdrawRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + result);
                    }
                    else
                    {
                        Console.WriteLine("Request failed: " + response.ReasonPhrase);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }




                _context.BorrowingTransactions.Add(transaction);
                book.is_Available = false; // Mark the book as unavailable.
                _context.SaveChanges();

                return Ok("Borrowing transaction created successfully");
            }
            catch (Exception ex)
            {

                return BadRequest("Error creating borrowing transaction: " + ex.Message);
            }
        }
        //[Authorize]
        [HttpPut]
        public IActionResult MarkBookAsRetburned(int transactionId)
        {
            var transaction = _context.BorrowingTransactions.FirstOrDefault(t => t.TraID == transactionId);
            try
            {
                if (transaction == null || transaction.return_date !=null)
                {
                    return NotFound(" transaction not found or book is already returned.");
                }
                else
                {
                    transaction.return_date = DateTime.Now;
                    var book = _context.bookManagements.FirstOrDefault(b => b.B_ID == transaction.B_ID);

                    if (book != null)
                    {
                        book.is_Available = true; // Mark the book as available.
                    }

                    _context.SaveChanges();
                    return Ok("Book returned succsessfully");
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete]
        public IActionResult Remove(int transactionId)
        {
            try
            {
                BorrowingTransactions borrowingTransactions = _context.BorrowingTransactions.FirstOrDefault(x => x.TraID == transactionId);

                if (borrowingTransactions != null)
                {
                    _context.BorrowingTransactions.Remove(borrowingTransactions);
                    _context.SaveChanges();

                    return Ok("Borrowing transaction removed successfully");
                }
                else
                {
                    return NotFound("Transaction not found");
                }
            }
            catch (Exception ex)
            {
                
                return BadRequest("Error removing borrowing transaction: " + ex.Message);
            }
        }


        [HttpGet("GetTransactionHistoryForPatron")]
        public IActionResult GetTransactionHistoryForPatron(int patronId)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true,
                };

                var patron = _context.patronManagements
                    .Include(x => x.borrowingTransactions)
                    .ThenInclude(x => x.BookManagement)
                    .FirstOrDefault(x => x.Pat_ID == patronId);

                if (patron != null)
                {
                    var transactionHistory = patron.borrowingTransactions
                    .OrderByDescending(transaction => transaction.borrowing_date)
                    .Select(transaction => new
                            {
                             TransactionID = transaction.TraID,
                             BookID = transaction.B_ID,
                             BookTitle = transaction.BookManagement.title,
                             BorrowDate = transaction.borrowing_date,
                             ReturnDate = transaction.return_date
                            })
                    .ToList();

                    return Ok(JsonSerializer.Serialize(transactionHistory, options));
                    //return Ok(transactionHistory);
                }
                else
                {
                    return NotFound("Patron not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving transaction history: " + ex.Message);
            }
        }

        
        [HttpGet("GetTransactionHistoryForBook")]
        public IActionResult GetTransactionHistoryForBook(int bookid)
        {
            try 
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true,
                };
                var Bh = _context.bookManagements.Include(x => x.borrowingTransactions)
                                             .ThenInclude(x => x.PatronManagement)
                                             .FirstOrDefault(x => x.B_ID == bookid);
                if (Bh != null)
                {
                   var BH = Bh.borrowingTransactions.OrderByDescending(B => B.borrowing_date)
                                                 .Select(B => new
                                                 {
                                                     TransactionID = B.TraID,
                                                     Booktitle = B.BookManagement.title,
                                                     PatronId = B.Pat_ID,
                                                     PatronName = B.PatronManagement.Name,
                                                     BorrowDate = B.borrowing_date,
                                                     returnDate = B.borrowing_date,
                                                 })
                                                .ToList();
                
                  return Ok(JsonSerializer.Serialize(BH,options));
                  //return Ok(BH);
                }
                else
                {
                   return NotFound("Book not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving transaction history: " + ex.Message);
            }

        }
       
        [HttpGet("AllTransaction")]
        public IActionResult GetAllTransactionHistory()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true,
                };

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
                else
                {
                    var TH= Ok(transactions.Select(x => new
                    {
                        TransactionID = x.TraID,
                        BookID = x.B_ID,
                        Booktitle = x.BookManagement.title,
                        PatronId = x.Pat_ID,
                        PatronName = x.PatronManagement.Name,
                        BorrowDate = x.borrowing_date,
                        returnDate = x.borrowing_date,
                    }));
                   return Ok(JsonSerializer.Serialize(TH, options));
                  // return Ok(TH);
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving transaction history: " + ex.Message);
            }
        }
        [HttpGet("GetByBorrowDate")]
        public IActionResult GetAllBorrowingsInDate(DateTime Borrowdate)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            try
            {
                var transactions = _context.BorrowingTransactions
                    .Include(t => t.BookManagement)
                    .Include(t => t.PatronManagement)
                    .Where(r => r.borrowing_date == Borrowdate.Date)
                    .ToList();

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No books borrowed this day ");
                }
                else
                {
                    var TH = Ok(transactions.Select(x => new
                    {
                        TransactionID = x.TraID,
                        BookID = x.B_ID,
                        Booktitle = x.BookManagement.title,
                        PatronId = x.Pat_ID,
                        PatronName = x.PatronManagement.Name,
                        BorrowDate = x.borrowing_date,
                        returnDate = x.borrowing_date,
                    }));
                    return Ok(JsonSerializer.Serialize(TH, options));
                   //return BadRequest(TH);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        [HttpGet("GetByReturnDate")]
        public IActionResult GetAllTransactionsByReturnDate(DateTime returnDate)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };

            try
            {
                var transactions = _context.BorrowingTransactions
                    .Include(t => t.BookManagement)
                    .Include(t => t.PatronManagement)
                    .Where(r => r.return_date.HasValue && r.return_date.Value.Date == returnDate.Date)
                    .ToList();

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No books returned this day ");
                }
                else
                {
                    var TH = Ok(transactions.Select(x => new
                    {
                        TransactionID = x.TraID,
                        BookID = x.B_ID,
                        Booktitle = x.BookManagement.title,
                        PatronId = x.Pat_ID,
                        PatronName = x.PatronManagement.Name,
                        BorrowDate = x.borrowing_date,
                        returnDate = x.borrowing_date,
                    }));
                    return Ok(JsonSerializer.Serialize(TH, options));
                    //return Ok(TH);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        [HttpGet("GetBorrowCount")]
        public IActionResult GetBorrowCountForBook(int bookId)
        {
            try
            {
                var borrowCount = _context.BorrowingTransactions
                    .Where(t => t.B_ID == bookId)
                    .Count();

                return Ok(borrowCount);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

    }
}
