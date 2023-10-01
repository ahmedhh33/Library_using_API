using System.ComponentModel.DataAnnotations;

namespace Library_web.Models
{
    public class PatronManagement
    {
        [Key]
        public int Pat_ID { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }

        public List<BorrowingTransactions> borrowingTransactions { get; set; }
    }
}
