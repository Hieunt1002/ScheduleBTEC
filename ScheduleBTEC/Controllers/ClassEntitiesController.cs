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
    public class ClassEntitiesController : Controller
    {
        private readonly ConnectDB _context;

        public ClassEntitiesController(ConnectDB context)
        {
            _context = context;
        }

        // GET: ClassEntities
        public async Task<IActionResult> Index()
        {
              return _context.ClassEntities != null ? 
                          View(await _context.ClassEntities.ToListAsync()) :
                          Problem("Entity set 'ConnectDB.ClassEntities'  is null.");
        }

        // GET: ClassEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassEntities == null)
            {
                return NotFound();
            }

            var classEntity = await _context.ClassEntities
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (classEntity == null)
            {
                return NotFound();
            }

            return View(classEntity);
        }

        // GET: ClassEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassId,className")] ClassEntity classEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classEntity);
        }

        // GET: ClassEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassEntities == null)
            {
                return NotFound();
            }

            var classEntity = await _context.ClassEntities.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }

        // POST: ClassEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassId,className")] ClassEntity classEntity)
        {
            if (id != classEntity.ClassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassEntityExists(classEntity.ClassId))
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
            return View(classEntity);
        }

        // GET: ClassEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClassEntities == null)
            {
                return NotFound();
            }

            var classEntity = await _context.ClassEntities
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (classEntity == null)
            {
                return NotFound();
            }

            return View(classEntity);
        }

        // POST: ClassEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassEntities == null)
            {
                return Problem("Entity set 'ConnectDB.ClassEntities'  is null.");
            }
            var classEntity = await _context.ClassEntities.FindAsync(id);
            if (classEntity != null)
            {
                _context.ClassEntities.Remove(classEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassEntityExists(int id)
        {
          return (_context.ClassEntities?.Any(e => e.ClassId == id)).GetValueOrDefault();
        }
    }
}
