using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleBTEC.Context;
using ScheduleBTEC.DTO;
using System.Globalization;

namespace ScheduleBTEC.Controllers
{
    public class SchedulesStaffController : Controller
    {
        private readonly ConnectDB _context;

        public SchedulesStaffController(ConnectDB context)
        {
            _context = context;
        }
      
            public async Task<IActionResult> Index(string week)
            {
                int id = (int)HttpContext.Session.GetInt32("ID");
                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            var shedulesStaff = (from s in _context.Schedules
                                join l in _context.Learns on s.LearnId equals l.LearnId
                                join cl in _context.ClassEntities on l.ClassId equals cl.ClassId
                                join t in _context.Teaches on l.TeachId equals t.TeachId
                                join c in _context.Courses on t.CourseId equals c.CourseId
                                select new LichhocDTo
                                {
                                    classname = cl.className,
                                    coursename = c.CourseName,
                                    learname = l.LearnName,
                                    teachername = t.TeachName,
                                    Datelear = s.DateLearn,
                                    timeLear = s.timelearn,
                                    ScheduleId = s.ScheduleId,
                                }
                                
                                ).ToList();


            return View(shedulesStaff);
            }
        }
    }
