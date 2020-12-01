using Syncfusion.EJ2.Base;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CareerInfo.Models;
using CareerInfo.Services;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CareerInfo.Temp;

namespace CareerInfo.Controllers
{
    [Authorize]
    public class JobsController : Controller
  {
        private readonly JobService _jobService;
        private readonly ModelContext modelContext;
       //private readonly UserManager<IdentityUser> userManager;
       //private readonly RoleManager<IdentityRole> roleManager;

        //, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager
        public JobsController(JobService jobService, ModelContext odbcontext)
        {
            _jobService = jobService;
           // this.userManager = userManager;
            //this.roleManager = roleManager;
            this.modelContext = odbcontext;
        }

        //public async Task<System.Collections.Generic.IList<string>> GetRoleAsync()
        //{


        //    var userId = User.FindFirstValue(ClaimTypes.Name);
        //    var user = await userManager.FindByNameAsync(userId);
        //    return await userManager.GetRolesAsync(user);
        //}

        //public async Task CreateUserRoles()
        //{

        //    IdentityResult roleResult;
        //    //Adding Admin Role
        //    var roleCheck = await roleManager.RoleExistsAsync("Admin");
        //    if (!roleCheck)
        //    {
        //        //create the roles and seed them to the database
        //        roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
        //        roleResult = await roleManager.CreateAsync(new IdentityRole("User"));
        //    }
        //    //Assign Admin role to the main User here we have given our newly registered 
        //    //login id for Admin management
        //    IdentityUser auser = await userManager.FindByEmailAsync("agentpieter@gmail.com");
        //    IdentityUser uuser = await userManager.FindByEmailAsync("pieterthepro@gmail.com");
        //    var User = new IdentityUser();
        //    await userManager.AddToRoleAsync(auser, "Admin");
        //    await userManager.AddToRoleAsync(uuser, "User");
        //}

        [HttpPost]
        public async Task<IActionResult> CreateFavourite(string JobId,string UserId)
        {

            Favouritejobs favouritejobs = new Favouritejobs();

            favouritejobs.Jobid = JobId;
            favouritejobs.Userid = UserId;
            modelContext.Add(favouritejobs);
            await modelContext.SaveChangesAsync();

            return Json(JobId);
        }
        public ActionResult Jobs()
        {
            


            return View();
        }
        public ActionResult Details(string id)
        {
            if(id == "")
            {
                return BadRequest();
            }
            try
            {
                Job job = _jobService.Get(ObjectId.Parse(id));
 
            //if job does not exist 
            if (job == null)
            {
                return NotFound();
            
            }

            return View(job);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        public ActionResult UrlDatasource([FromBody]DataManagerRequest dm)
        {

            IEnumerable DataSource = _jobService.Get();



            DataOperations operation = new DataOperations();
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Job>().Count();
            if (dm.Skip != 0)//Paging
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return Json(new { result = DataSource, count = count });
        }
        public ActionResult Insert([FromBody]CRUDModel<Job> value)
        {
            //do stuff
            _jobService.Create(value.Value);

            return Json(value);

        }
        public ActionResult Update([FromBody]CRUDModel<Job> value)
        {
            var ord = value.Value;

            Job val = _jobService.Get(value.Value.Id);

            if (val == null)
            {
                return NotFound();
            }

            //val.Id = ord.Value.Id;
            val.Requirements.Experience = ord.Requirements.Experience;
            val.Requirements.Education = ord.Requirements.Education;
            val.Requirements.Languages = ord.Requirements.Languages;
            // val.Company.name = ord.Company.name;
            val.Date_posted = ord.Date_posted;
            val.Job_title = ord.Job_title;
            val.payment_type.amount = ord.payment_type.amount;
            //val.Job_id = ord.Job_id;
            val.Spoken_Languages = ord.Spoken_Languages;
            val.Site_link = ord.Site_link;

            _jobService.Update(value.Value.Id, val);

            return Json(value);
        }
        public ActionResult Delete([FromBody]CRUDModel<Job> value)
        {
            //do stuff

            var job = _jobService.Get(ObjectId.Parse(value.Key.ToString()));

            if (job == null)
            {
                return NotFound();
            }

            _jobService.Remove(ObjectId.Parse(value.Key.ToString()));
            return Json(job);
        }
    }
}