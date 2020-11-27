using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CareerInfo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using CareerInfo.Services;

namespace CareerInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly JobService _jobService;

        public HomeController(JobService jobService)
        {
            _jobService = jobService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [Authorize]
        public IActionResult Dashboard()
        {
            long count = _jobService.Count();
            List<double> average = _jobService.Average();

            ViewData["JobCount"] = count;
            ViewData["AverageSalary"] = Math.Round(average[0], 2, MidpointRounding.AwayFromZero);
            ViewData["FavouritedJobs"] = "7";
            ViewData["TotalCompanies"] = "90";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
