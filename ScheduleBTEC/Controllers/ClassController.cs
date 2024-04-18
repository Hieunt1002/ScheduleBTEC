using Microsoft.AspNetCore.Mvc;
using ScheduleBTEC.Context;
using ScheduleBTEC.DTO;
using ScheduleBTEC.Models;

namespace ScheduleBTEC.Controllers
{
    public class ClassController : Controller
    {
        private readonly ConnectDB db = new ConnectDB();

        public IActionResult Index()
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            var classstudent = db.ClassEntities;
            return View(classstudent);
        }
        public ActionResult AddClass()
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddClass(ClassStudent model)
        {
            if (model != null)
            {
                var classstudent = new ClassEntity
                {
                    className = model.className,

                };
                db.ClassEntities.Add(classstudent);
                db.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        public ActionResult EditClass(int id)
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            var classstudent = db.ClassEntities.Find(id);
            return View(classstudent);
        }
        [HttpPost]
        public IActionResult EditClass(ClassEntity model)
        {
            db.ClassEntities.Attach(model);
            db.Update(model);


            db.SaveChanges();



            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteAcc(int id)
        {
            var item = db.ClassEntities.Find(id);
            if (item != null)
            {
                /*var DeleteItem=db.Categories.Attach(item);*/
                db.ClassEntities.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
