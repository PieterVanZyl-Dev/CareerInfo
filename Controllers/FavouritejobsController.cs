using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerInfo.Temp;
using CareerInfo.Models;
using CareerInfo.Services;
using MongoDB.Bson;

namespace CareerInfo.Views
{
    public class FavouritejobsController : Controller
    {
        private readonly ModelContext _context;
        private readonly JobService _jobService;

        public FavouritejobsController(ModelContext context, JobService jobService)
        {
            _context = context;
            _jobService = jobService;

    }

        // GET: Favouritejobs
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Favouritejobs.Include(f => f.User);
            var favouritejoblist = await modelContext.Where(f => f.User.UserName == User.Identity.Name).ToListAsync();
            List<ListJob> FavouritedJobs = new List<ListJob>();
            foreach (var favouritejob in favouritejoblist)
            {
                var job = _jobService.Get(ObjectId.Parse(favouritejob.Jobid));
                ListJob listjob = new ListJob();
                listjob.Job_title = job.Job_title;
                listjob.company_name = job.company_name;
                listjob.Date_posted = job.Date_posted;
                listjob.Id = job.Id;
                listjob.Site_link = job.Site_link;
                listjob.location = job.location;
                listjob.Spoken_Languages = job.Spoken_Languages;
                listjob.payment_type = job.payment_type;
                listjob.Requirements = job.Requirements;
                listjob.job_Description = job.job_Description;
                listjob.FavouriteId = favouritejob.Favouriteid;
                FavouritedJobs.Add(listjob);
                //favouritejob.Jobid 
            }

            return View(FavouritedJobs);
        }

        // GET: Favouritejobs/Details/5
        public IActionResult Details(string id)
        {
            if (id == "")
            {
                return BadRequest();
            }
            try
            {
                Job job = _jobService.Get(ObjectId.Parse(id));
                ResponseJob rjob = new ResponseJob();
                rjob.Job_title = job.Job_title;
                rjob.company_name = job.company_name;
                rjob.Date_posted = job.Date_posted;
                rjob.Id = job.Id;
                rjob.Site_link = job.Site_link;
                rjob.location = job.location;
                rjob.Spoken_Languages = job.Spoken_Languages;
                rjob.payment_type = job.payment_type;
                rjob.Requirements = job.Requirements;
                rjob.job_Description = job.job_Description;

                List<string> splitedu = job.Requirements.Education.Split(";").ToList();
                List<string> splitlang = job.Requirements.Languages.Split(";").ToList();
                rjob.plangs = splitlang;
                rjob.erequirements = splitedu;

                //if job does not exist 
                if (job == null)
                {
                    return NotFound();

                }

                return View(rjob);
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
