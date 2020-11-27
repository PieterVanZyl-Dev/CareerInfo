using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerInfo.Temp;

namespace CareerInfo.Views
{
    public class AspNetUserRolesController : Controller
    {
        private readonly ModelContext _context;

        public AspNetUserRolesController(ModelContext context)
        {
            _context = context;
        }

        // GET: AspNetUserRoles
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.AspNetUserRoles.Include(a => a.Role).Include(a => a.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: AspNetUserRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRoles = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            return View(aspNetUserRoles);
        }

        // GET: AspNetUserRoles/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: AspNetUserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] AspNetUserRoles aspNetUserRoles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUserRoles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRoles.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRoles.UserId);
            return View(aspNetUserRoles);
        }

        // GET: AspNetUserRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRoles.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRoles.UserId);
            return View(aspNetUserRoles);
        }

        // POST: AspNetUserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,RoleId")] AspNetUserRoles aspNetUserRoles)
        {
            if (id != aspNetUserRoles.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserRoles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserRolesExists(aspNetUserRoles.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRoles.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRoles.UserId);
            return View(aspNetUserRoles);
        }

        // GET: AspNetUserRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRoles = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            return View(aspNetUserRoles);
        }

        // POST: AspNetUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);
            _context.AspNetUserRoles.Remove(aspNetUserRoles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserRolesExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }
    }
}
