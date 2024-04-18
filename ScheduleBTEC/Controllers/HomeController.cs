using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ScheduleBTEC.Context;
using ScheduleBTEC.DTO;
using ScheduleBTEC.Models;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace ScheduleBTEC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConnectDB _context;

        public HomeController(ILogger<HomeController> logger, ConnectDB context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Login(CUser model)
        {
            var user = _context.Users.SingleOrDefault(kh => kh.Email == model.Email && kh.Password == model.Password);

            if (user?.Email != model.Email || user?.Password != model.Password)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
            if (user == null)
            {
                return View();
            }
            HttpContext.Session.SetInt32("ID", user.UserId);
            HttpContext.Session.SetString("Fullname", user.Fullname);
            HttpContext.Session.SetString("Role", user.Role);

            if (user.Role == "4")
            {
                return RedirectToAction("IndexAccount", "Admin");
            }
            else if (user.Role == "3")
            {
                return RedirectToAction("IndexAccount", "Admin");
            }
            else if (user.Role == "1")
            {
                return RedirectToAction("Index", "Schedules");
            }
            else if (user.Role == "2")
            {
                return RedirectToAction("Index", "Schedules");
            }

            return RedirectToAction("Login");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}