using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CareerInfo.Models;
namespace CareerInfo.Controllers
{

   public class TestGridController : Controller
   {
        private MongoDBContext _context;

		public TestGridController(MongoDBContext Context)
		{
            this._context=Context;
		}

        public ActionResult Testing()
        {

            ViewBag.dataSource = _context.Jobs.ToList();

            return View();
        }

    }
}