using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library_web.Models
{
    public class BorrowingTransactions
    {
        [Key]
        public int TraID { get; set; }
        [Required]
        public DateTime borrowing_date { get; set; }

        public DateTime? return_date { get; set; }


        [ForeignKey("BookManagement")]
        public int B_ID { get; set; }
        public BookManagement BookManagement { get; set; }

        [ForeignKey("PatronManagement")]
        public int Pat_ID { get; set; }
        public PatronManagement PatronManagement { get; set; }
    }
}
