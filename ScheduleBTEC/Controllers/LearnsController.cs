using System;
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Learns == null)
            {
                return NotFound();
            }

            var learn = await _context.Learns.FindAsync(id);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnId,LearnName,TeachId,ClassId")] Learn learn)
        {
            if (id != learn.LearnId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnExists(learn.LearnId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.ClassEntities, "ClassId", "className", learn.ClassId);
            ViewData["TeachId"] = new SelectList(_context.Teaches, "TeachId", "TeachName", learn.TeachId);
            return View(learn);
        }

        // GET: Learns/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Learns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Learns == null)
            {
                return Problem("Entity set 'ConnectDB.Learns'  is null.");
            }
            var learn = await _context.Learns.FindAsync(id);
            if (learn != null)
            {
                _context.Learns.Remove(learn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnExists(int id)
        {
            return (_context.Learns?.Any(e => e.LearnId == id)).GetValueOrDefault();
        }
    }
}
