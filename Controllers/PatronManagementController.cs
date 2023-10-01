using Library_web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronManagementController : ControllerBase
    {
        public static ApplicationDbContext _context;

        public PatronManagementController(ApplicationDbContext DB)
        {
            _context = DB;
        }

        [HttpPost]
        public static void AddPatron(string nam, string emailAderss)
        {
            PatronManagement patronManagement = new PatronManagement
            {
                Name = nam,
                EmailAddress = emailAderss
            };
            _context.patronManagements.Add(patronManagement);
            _context.SaveChanges();
        }
        [HttpDelete]
        public static void Remove(int ID)
        {
            PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.Pat_ID == ID);

            if (patrnmanagement != null)
            {
                _context.patronManagements.Remove(patrnmanagement);
                _context.SaveChanges();

                Console.WriteLine("PATRON Removes SUccessfully");
            }
            else
            {
                Console.WriteLine("Book Not Found");
            }
        }
        [HttpPut]
        public static void UpdateBook(int ID, string Name, string EmailAdress)
        {
            PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.Pat_ID == ID);

            if (patrnmanagement != null)
            {
                patrnmanagement.Name = Name;
                patrnmanagement.EmailAddress = EmailAdress;
                _context.Update(patrnmanagement);
                _context.SaveChanges();

                Console.WriteLine("Patron updated SUccessfully");
            }
            else
            {
                Console.WriteLine("Book Not Found");
            }
        }

        [HttpGet]
        public static PatronManagement GetPatronByName(string Name)
        {
            PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.Name == Name);
            return patrnmanagement;
        }
        [HttpGet("GetEmployeeByEmail")]
        public static PatronManagement GetPatronByEmail(string EmailAdress)
        {
            PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.EmailAddress == EmailAdress);
            return patrnmanagement;

        }
    }
}
