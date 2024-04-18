using Microsoft.AspNetCore.Mvc;
using ScheduleBTEC.Context;
using ScheduleBTEC.DTO;
using ScheduleBTEC.Models;

namespace ScheduleBTEC.Controllers
{
    public class CourseController : Controller
    {
        private readonly ConnectDB db = new ConnectDB();

        public IActionResult Index()
        {
            var course = db.Courses;
            return View(course);
        }
        public ActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCourse(CCourse model )
        {
           if( model != null )
            {
                var course = new Course
                {
                    CourseName = model.CourseName,
                };
                db.Courses.Add( course );
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditCourse(int id)
        {
            var course = db.Courses.Find( id );
            return View(course);
        }
        [HttpPost]
        public IActionResult EditCourse(Course model)
        {
            db.Courses.Attach(model);
            db.Update(model);


            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            var item = db.Courses.Find(id);
            if (item != null)
            {
                /*var DeleteItem=db.Categories.Attach(item);*/
                db.Courses.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
