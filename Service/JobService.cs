using CareerInfo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerInfo.Services
{
    public class JobService
    {
        private readonly IMongoCollection<Job> _jobs;

        public JobService(IMongoDBsettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _jobs = database.GetCollection<Job>(settings.JobsCollectionName);
        }

        public List<Job> Get() =>
            _jobs.Find(job => true).ToList();

        public async Task<List<Job>> GetAsync()
        {
            return await _jobs.Find(job => true).ToListAsync();
        }

        public List<BsonDocument> GetLangauges()
        {
            return _jobs.Find(FilterDefinition<Job>.Empty)
            .Project(Builders<Job>.Projection.Include("Requirements.Languages").Exclude("_id")).ToList();
        }

        public Job Get(ObjectId id) =>
            _jobs.Find<Job>(job => job.Id == id).FirstOrDefault();

        public long Count() =>
        _jobs.CountDocuments(job => true);

        public Job Create(Job job)
        {
            _jobs.InsertOne(job);
            return job;
        }

        public List<double> Average()
        {

            var q = from doc in _jobs.AsQueryable()
                    where doc.payment_type.amount > 1000
                    where doc.payment_type.amount < 999999
                    group doc by (Job)null into gr
                    select new 
                    {
                        Avg = (double)gr.Average(x => x.payment_type.amount),
                        Min = gr.Min(x => x.payment_type.amount),
                        Max = gr.Max(x => x.payment_type.amount)
                    };

            var result = q.First();

            List<double> AverageMinMax = new List<double>();
            AverageMinMax.Add(result.Avg);
            AverageMinMax.Add(result.Min);
            AverageMinMax.Add(result.Max);

            return AverageMinMax;
        }

        public void Update(ObjectId id, Job jobIn) =>
            _jobs.ReplaceOne(job => job.Id == id, jobIn);

        public void Remove(ObjectId id) =>
            _jobs.DeleteOne(job => job.Id == id);
    }


}