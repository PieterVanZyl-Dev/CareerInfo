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
    public class FavouritejobsController : Controller
    {
        private readonly ModelContext _context;

        public FavouritejobsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Favouritejobs
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Favouritejobs.Include(f => f.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Favouritejobs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouritejobs = await _context.Favouritejobs
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Favouriteid == id);
            if (favouritejobs == null)
            {
                return NotFound();
            }

            return View(favouritejobs);
        }

        // GET: Favouritejobs/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Favouritejobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Favouriteid,Jobid,Userid")] Favouritejobs favouritejobs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favouritejobs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", favouritejobs.Userid);
            return View(favouritejobs);
        }

        // GET: Favouritejobs/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouritejobs = await _context.Favouritejobs.FindAsync(id);
            if (favouritejobs == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", favouritejobs.Userid);
            return View(favouritejobs);
        }

        // POST: Favouritejobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Favouriteid,Jobid,Userid")] Favouritejobs favouritejobs)
        {
            if (id != favouritejobs.Favouriteid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favouritejobs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouritejobsExists(favouritejobs.Favouriteid))
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
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", favouritejobs.Userid);
            return View(favouritejobs);
        }

        // GET: Favouritejobs/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouritejobs = await _context.Favouritejobs
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Favouriteid == id);
            if (favouritejobs == null)
            {
                return NotFound();
            }

            return View(favouritejobs);
        }

        // POST: Favouritejobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var favouritejobs = await _context.Favouritejobs.FindAsync(id);
            _context.Favouritejobs.Remove(favouritejobs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavouritejobsExists(decimal id)
        {
            return _context.Favouritejobs.Any(e => e.Favouriteid == id);
        }
    }
}
