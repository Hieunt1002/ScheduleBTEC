using Microsoft.AspNetCore.Mvc;
using ScheduleBTEC.Context;
using ScheduleBTEC.DTO;
using Microsoft.IdentityModel.Tokens;
using ScheduleBTEC.Models;
using Microsoft.EntityFrameworkCore;

namespace ScheduleBTEC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ConnectDB db = new ConnectDB();

        public ActionResult IndexAccount()
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            var user = db.Users.ToList();
            return View(user);
        }
        public ActionResult CreatAccount()
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult CreatAccount(CUser model)
        {
            if (model != null)
            {
                var cuser = new Users
                {
                    Email = model.Email,
                    Password = model.Password,
                    Fullname = model.Fullname,
                    Phone = model.Phone,
                    Role = model.Role,
                };

                db.Users.Add(cuser);
                db.SaveChanges();
            }
          
            return RedirectToAction("IndexAccount");
        }
        public ActionResult EditAccount(int id)
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            var item = db.Users.Find(id);

            return View(item);
        }
        [HttpPost]
        public IActionResult EditAccount(Users model)
        {
            db.Users.Attach(model);
            db.Update(model);

           
            db.SaveChanges();


            
            return RedirectToAction("IndexAccount");
        }
        [HttpPost]
        public ActionResult DeleteAcc(int id)
        {
            var item = db.Users.Find(id);
            if (item != null)
            {
                /*var DeleteItem=db.Categories.Attach(item);*/
                db.Users.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
