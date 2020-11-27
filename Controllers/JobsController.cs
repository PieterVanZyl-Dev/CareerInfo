﻿using Syncfusion.EJ2.Base;
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

namespace CareerInfo.Controllers
{
    [Authorize]
    public class JobsController : Controller
  {
        private readonly JobService _jobService;

        public JobsController(JobService jobService)
        {
            _jobService = jobService;
        }


        public ActionResult Jobs()
        {
            ViewData["IsAdmin"] = User.IsInRole("Admin");
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
            val.Job_id = ord.Job_id;
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