using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace CareerInfo.Models
{

    public class Requirements
    {
        public string Experience { get; set; }
        public string Languages { get; set; }
        public string Education { get; set; }
    }

    public class payment_type
    {
        public string type { get; set; }
        public int amount { get; set; }
    }




    [System.ComponentModel.DataAnnotations.Schema.Table("JobCollection")]
    public class Job
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public Requirements Requirements { get; set; }
        public string company_name { get; set; }
        public DateTime Date_posted { get; set; }
        public string Job_title { get; set; }
        public string Search_indeed { get; set; }
        public string location { get; set; }
        public int Salary { get; set; }
        public string Job_id { get; set; }
        public string Spoken_Languages { get; set; }
        public string Site_link { get; set; }
        public payment_type payment_type { get; set; }
    }


}
