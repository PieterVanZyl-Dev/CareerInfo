using System;
using System.Collections.Generic;

namespace CareerInfo.Temp
{
    public partial class Favouritejobs
    {
        public decimal Favouriteid { get; set; }
        public string Jobid { get; set; }
        public string Userid { get; set; }

        public AspNetUsers User { get; set; }
    }
}
