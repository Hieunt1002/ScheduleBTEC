﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleBTEC.Context;
using ScheduleBTEC.Models;

namespace ScheduleBTEC.Controllers
{
    public class LearnsController : Controller
    {
        private readonly ConnectDB _context;

        public LearnsController(ConnectDB context)
        {
            _context = context;
        }

        // GET: Learns
        public async Task<IActionResult> Index()
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            var connectDB = _context.Learns.Include(l => l.ClassEntity).Include(l => l.Teach);
            return View(await connectDB.ToListAsync());
        }

        // GET: Learns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Learns == null)
            {
                return NotFound();
            }

            var learn = await _context.Learns
                .Include(l => l.ClassEntity)
                .Include(l => l.Teach)
                .FirstOrDefaultAsync(m => m.LearnId == id);
            if (learn == null)
            {
                return NotFound();
            }

            return View(learn);
        }

        // GET: Learns/Create
        public IActionResult Create()
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            ViewData["ClassId"] = new SelectList(_context.ClassEntities, "ClassId", "className");
            ViewData["TeachId"] = new SelectList(_context.Teaches, "TeachId", "TeachName");
            return View();
        }

        // POST: Learns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnId,LearnName,TeachId,ClassId")] Learn learn)
        {
           
                _context.Add(learn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
           
           
        }

        // GET: Learns/Edit/5
        public  IActionResult Edit(int? id)
        {
            string roleIdString = HttpContext.Session.GetString("Role");
            if (roleIdString == null || (roleIdString != "4" && roleIdString != "3"))
            {
                return Redirect("/Home/Login");
            }
            
            var learn =  _context.Learns.Find(id);
            if (learn == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.ClassEntities, "ClassId", "className", learn.ClassId);
            ViewData["TeachId"] = new SelectList(_context.Teaches, "TeachId", "TeachName", learn.TeachId);
            return View(learn);
        }

        // POST: Learns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public IActionResult Edit( Learn learn)
        {


            _context.Learns.Attach(learn);
            _context.Update(learn);
                     _context.SaveChanges();
                
               
                return RedirectToAction("Index");
            
          
        }

        // GET: Learns/Delete/5
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            var item = _context.Learns.Find(id);
            if (item != null)
            {
                /*var DeleteItem=db.Categories.Attach(item);*/
                _context.Learns.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        private bool LearnExists(int id)
        {
          return (_context.Learns?.Any(e => e.LearnId == id)).GetValueOrDefault();
        }
    }
}
