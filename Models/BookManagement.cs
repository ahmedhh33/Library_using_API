using System.ComponentModel.DataAnnotations;

namespace Library_web.Models
{
    public class BookManagement
    {
        [Key]
        public int B_ID { get; set; }

        [Required]
        public string title { get; set; }
        [Required]
        public string author { get; set; }
        [Required]
        public int publication_year { get; set; }
        [Required]
        public bool is_Available { get; set; }

        public List<BorrowingTransactions> borrowingTransactions { get; set; }
    }
}
