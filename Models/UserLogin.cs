using System.ComponentModel.DataAnnotations;

namespace Library_web.Models
{
    public class UserLogin
    {
        [Key]
       public int UserId { get; set; }
       public string UserName { get; set; }
       public string Password { get; set; }
       public string Email { get; set; }
        
    }
}
