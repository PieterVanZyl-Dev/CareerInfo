using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Threading.Tasks;

namespace CareerInfo.Models
{
    public partial class MongoDBContext : DbContext
    {
        public MongoDBContext()
        {
        }

        public MongoDBContext(DbContextOptions<MongoDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }
       public DbSet<Job> Jobs { set; get; }


    }
}

