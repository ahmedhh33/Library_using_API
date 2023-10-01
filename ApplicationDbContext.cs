using Library_web.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_web
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            options.UseSqlServer("Data Source=(local);Initial Catalog=library_EFCORE; Integrated Security=true; TrustServerCertificate=True");
        }
        public DbSet<BorrowingTransactions> BorrowingTransactions { get; set; }
        public DbSet<PatronManagement> patronManagements { get; set; }
        public DbSet<BookManagement> bookManagements { get; set; }

    }

}
