using Library_web.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_web
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)//use for appps.json
            : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //its now used in appstings.json in case you want to chang server
            //options.UseSqlServer("Data Source=(local);Initial Catalog=library_EFCORE; Integrated Security=true; TrustServerCertificate=True");
        }
        public DbSet<BorrowingTransactions> BorrowingTransactions { get; set; }
        public DbSet<PatronManagement> patronManagements { get; set; }
        public DbSet<BookManagement> bookManagements { get; set; }
        public DbSet<UserLogin> userLogins { get; set;}
    }

}
