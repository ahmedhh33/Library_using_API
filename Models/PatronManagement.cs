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
        [Required]
        public int Age { get; set; }
        [Required]
        public string password { get; set; }

        public List<BorrowingTransactions> borrowingTransactions { get; set; }
    }
}
