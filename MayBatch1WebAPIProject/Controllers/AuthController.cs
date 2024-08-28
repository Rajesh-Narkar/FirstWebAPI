using Microsoft.AspNetCore.Mvc;
using MayBatch1WebAPIProject.Models;
using MayBatch1WebAPIProject.Data;

namespace MayBatch1WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        ApplicationDbContext db;
        public AuthController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("SignIn")]
        public IActionResult Signin(Login log)
        {
            var data = db.users.Where(x => x.Username.Equals(log.Username) && x.Password.Equals(log.Password)).SingleOrDefault();
            if (data != null)
            {
                //var data=db.login.
                //var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, data.Username) }, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);
                //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //HttpContext.Session.SetString("Username", data.Username);
                return Ok("Login Successfully");
                //return RedirectToAction("Index", "Auth");
            }
            else
            {
                //TempData["error"] = "Invalid Credentials!!";
                //return RedirectToAction("SignIn");
                return Ok("Invalid Credentials");
            }
        }
    }
}
