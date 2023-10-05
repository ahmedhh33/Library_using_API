using Library_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static ApplicationDbContext _context;
        public LoginController(ApplicationDbContext db)
        {
            _context = db;
        }

        [HttpPost]
        public IActionResult post(string email, string password)
        {
            UserLogin user = _context.userLogins.Where(n => n.Email == email && n.Password == password).FirstOrDefault();
            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var data = new List<Claim>();
                data.Add(new Claim("UserName", user.UserName));


                var token = new JwtSecurityToken(
                issuer: "ahmed",
                audience: "TRA",
                claims: data,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials

                );

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));

            }
            else
            {
                return Unauthorized("the user doesn't exist");
            }

        }
    }
}
