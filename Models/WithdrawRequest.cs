namespace Library_web.Models
{
    public class WithdrawRequest
    {
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public int AccountHolderID { get; set; }
    }
}
