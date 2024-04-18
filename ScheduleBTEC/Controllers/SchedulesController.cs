using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleBTEC.Context;
using ScheduleBTEC.DTO;
using ScheduleBTEC.Models;

namespace ScheduleBTEC.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ConnectDB _context;

        public SchedulesController(ConnectDB context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index(string week)
        {
            int id = (int)HttpContext.Session.GetInt32("ID");
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            DateTime startOfWeek;
            DateTime endOfWeek;
            if (!string.IsNullOrEmpty(week))
            {
                string[] parts = week.Split("-W");
                if (parts.Length != 2 || !int.TryParse(parts[0], out int year) || !int.TryParse(parts[1], out int weekNumber))
                {
                    return BadRequest("Invalid week format. Please provide the week in the format 'yyyy-Www'.");
                }

                DateTime jan1 = new DateTime(year, 1, 1);
                int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
                DateTime firstMonday = jan1.AddDays(daysOffset);
                int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                startOfWeek = firstMonday.AddDays((weekNumber - firstWeek) * 7);
                endOfWeek = startOfWeek.AddDays(6);
            }
            else
            {
                DateTime today = DateTime.Today;
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek) % 7;
                startOfWeek = today.AddDays(daysUntilMonday);
                endOfWeek = startOfWeek.AddDays(6);
            }

            if (user.Role == null)
            {
                // Handle the case where user role is null
                return BadRequest("User role is not defined.");
            }

            if (user.Role == "1")
            {
                var connectDB = (from s in _context.Schedules
                                 join l in _context.Learns on s.LearnId equals l.LearnId
                                 join cl in _context.ClassEntities on l.ClassId equals cl.ClassId
                                 join t in _context.Teaches on l.TeachId equals t.TeachId
                                 join c in _context.Courses on t.CourseId equals c.CourseId
                                 where t.UserId == id && s.DateLearn >= startOfWeek && s.DateLearn <= endOfWeek
                                 select new ViewScheduleDTO
                                 {
                                     ScheduleId = s.ScheduleId,
                                     coursename = c.CourseName,
                                     DateLearn = s.DateLearn,
                                     timelearn = s.timelearn,
                                     classname = cl.className,
                                     startdate = startOfWeek,
                                     enddate = endOfWeek,
                                     status = !_context.Attendances.Any(a => a.ScheduleId == s.ScheduleId),
                                     role = user.Role
                                 }).ToList();


                return View(connectDB);
            }
            else
            {
                var connectDB = (from s in _context.Schedules
                                 join l in _context.Learns on s.LearnId equals l.LearnId
                                 join cl in _context.ClassEntities on l.ClassId equals cl.ClassId
                                 join st in _context.Studys on l.LearnId equals st.LearnId
                                 join t in _context.Teaches on l.TeachId equals t.TeachId
                                 join c in _context.Courses on t.CourseId equals c.CourseId
                                 where st.UserId == id && s.DateLearn >= startOfWeek && s.DateLearn <= endOfWeek
                                 select new ViewScheduleDTO
                                 {
                                     ScheduleId = s.ScheduleId,
                                     coursename = c.CourseName,
                                     DateLearn = s.DateLearn,
                                     timelearn = s.timelearn,
                                     classname = cl.className,
                                     startdate = startOfWeek,
                                     enddate = endOfWeek,
                                     status = (from a in _context.Attendances
                                               where a.ScheduleId == s.ScheduleId && a.UserId == id
                                               select a.status).FirstOrDefault(),
                                     role = user.Role
                                 }).ToList();
                return View(connectDB);
            }
        }
        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }
            var schedule = from s in _context.Schedules
                           join l in _context.Learns on s.LearnId equals l.LearnId
                           join st in _context.Studys on s.LearnId equals st.LearnId
                           join u in _context.Users on st.UserId equals u.UserId
                           join c in _context.ClassEntities on l.ClassId equals c.ClassId
                           join a in _context.Attendances on s.ScheduleId equals a.ScheduleId into gj
                           from a in gj.DefaultIfEmpty()
                           where s.ScheduleId == id
                           select new AttendanceDTO
                           {
                               Schedule = s,
                               Learn = l,
                               Users = u,
                               ClassEntity = c,
                               Attendance = a
                           };
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["LearnId"] = new SelectList(_context.Learns, "LearnId", "LearnName");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("startDate,endDate,session,timelearn,LearnId")] ScheduleDTO schedule)
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || roleIdString != "4" || roleIdString != "3")
            {
                return Redirect("/Home/Login");
            }
            if (ModelState.IsValid)
            {
                for (DateTime date = schedule.startDate; date <= schedule.endDate; date = date.AddDays(1))
                {
                    DayOfWeek dayOfWeek = date.DayOfWeek;
                    int dayOfWeekNumber = (int)dayOfWeek;
                    var check = _context.Schedules.FirstOrDefault(c => c.DateLearn == date && c.timelearn == schedule.timelearn);
                    if (check != null)
                    {
                        ViewData["LearnId"] = new SelectList(_context.Learns, "LearnId", "LearnId", schedule.LearnId);
                        return View(schedule);
                    }
                    else
                    {
                        if (dayOfWeekNumber == schedule.session)
                        {
                            var sch = new Schedule
                            {
                                DateLearn = date,
                                timelearn = schedule.timelearn,
                                LearnId = schedule.LearnId
                            };

                            _context.Add(sch);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["LearnId"] = new SelectList(_context.Learns, "LearnId", "LearnId", schedule.LearnId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || roleIdString != "1" )
            {
                return Redirect("/Home/Login");
            }
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }
            var schedule = (from s in _context.Schedules
                            join l in _context.Learns on s.LearnId equals l.LearnId
                            join st in _context.Studys on s.LearnId equals st.LearnId
                            join u in _context.Users on st.UserId equals u.UserId
                            join c in _context.ClassEntities on l.ClassId equals c.ClassId
                            join a in _context.Attendances on new { s.ScheduleId, u.UserId } equals new { a.ScheduleId, a.UserId } into gj
                            from a in gj.DefaultIfEmpty()
                            where s.ScheduleId == id
                            select new AttendanceDTO
                            {
                                Schedule = s,
                                Learn = l,
                                Users = u,
                                ClassEntity = c,
                                Attendance = a
                            }).ToList();

            if (!schedule.Any())
            {
                return NotFound();
            }

            return View(schedule);
        }


        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int[] id, int[] userid, bool[] status, int[] attend)
        {
            try
            {
                var check = _context.Attendances.FirstOrDefault(c => c.ScheduleId == id[0]);
                if (check == null)
                {
                    for (int i = 0; i < userid.Length; i++)
                    {
                        var att = new Attendance
                        {
                            UserId = userid[i],
                            ScheduleId = id[i],
                            status = status[i]
                        };
                        _context.Add(att);
                        await _context.SaveChangesAsync();
                    }

                }
                else
                {
                    for (int i = 0; i < userid.Length; i++)
                    {
                        var a = _context.Attendances.FirstOrDefault(n => n.AttendanceId == attend[i]);
                        if (a != null)
                        {
                            a.status = status[i];
                            _context.Entry(a).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Learn)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'ConnectDB.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return (_context.Schedules?.Any(e => e.ScheduleId == id)).GetValueOrDefault();
        }
    }
}