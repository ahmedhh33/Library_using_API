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
        public IActionResult AddPatron(string name, string emailAddress, int age)
        {
            try
            {
                PatronManagement patronManagement = new PatronManagement
                {
                    Name = name,
                    EmailAddress = emailAddress,
                    Age = age
                };

                _context.patronManagements.Add(patronManagement);
                _context.SaveChanges();

                return Ok("Patron added successfully");
            }
            catch (Exception ex)
            {
               
                return BadRequest("Error adding patron: " + ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Remove(int ID)
        {
            try
            {
                PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.Pat_ID == ID);

                if (patrnmanagement != null)
                {
                    _context.patronManagements.Remove(patrnmanagement);
                    _context.SaveChanges();

                    return Ok("Patron removed successfully");
                }
                else
                {
                    return NotFound("Patron not found");
                }
            }
            catch (Exception ex)
            {
                
                return BadRequest("Error removing patron: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdatePatron(int ID, string Name, string EmailAddress, int Age)
        {
            try
            {
                PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.Pat_ID == ID);

                if (patrnmanagement != null)
                {
                    patrnmanagement.Name = Name;
                    patrnmanagement.EmailAddress = EmailAddress;
                    patrnmanagement.Age = Age;

                    _context.Update(patrnmanagement);
                    _context.SaveChanges();

                    return Ok("Patron updated successfully");
                }
                else
                {
                    return NotFound("Patron not found");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the operation.
                // You can log the exception or return an error message.
                return BadRequest("Error updating patron: " + ex.Message);
            }
        }


        [HttpGet("GetPatronByName")]
        public IActionResult GetPatronByName(string Name)
        {
            PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.Name == Name);

            if (patrnmanagement != null)
            {
                return Ok(patrnmanagement);
            }
            else
            {
                return NotFound("Patron not found");
            }
        }

        [HttpGet("GetPatronByEmail")]
        public IActionResult GetPatronByEmail(string EmailAddress)
        {
            PatronManagement patrnmanagement = _context.patronManagements.SingleOrDefault(x => x.EmailAddress == EmailAddress);

            if (patrnmanagement != null)
            {
                return Ok(patrnmanagement);
            }
            else
            {
                return NotFound("Patron not found");
            }
        }

        [HttpGet("GetAllPatrons")]
        public IActionResult GetAllPatrons()
        {
            List<PatronManagement> patronManagements = _context.patronManagements.ToList();

            if (patronManagements.Count > 0)
            {
                return Ok(patronManagements);
            }
            else
            {
                return NotFound("No patrons found");
            }
        }

        [HttpGet("GetAllPatronsWithAgePeriod")]
        public IActionResult GetAllPatronsWithAgePeriod(int Age1, int Age2)
        {
            List<PatronManagement> patronManagements = _context.patronManagements
                .Where(x => x.Age >= Age1 && x.Age <= Age2)
                .ToList();

            if (patronManagements.Count > 0)
            {
                return Ok(patronManagements);
            }
            else
            {
                return NotFound("No patrons found in the specified age range");
            }
        }


    }
}
