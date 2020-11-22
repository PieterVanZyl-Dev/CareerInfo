using CareerInfo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

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

        public List<BsonDocument> GetLangauges()
        {
            return _jobs.Find(FilterDefinition<Job>.Empty)
            .Project(Builders<Job>.Projection.Include("Requirements.Languages").Exclude("_id")).ToList();
        }

        public Job Get(ObjectId id) =>
            _jobs.Find<Job>(job => job.Id == id).FirstOrDefault();

        public Job Create(Job job)
        {
            _jobs.InsertOne(job);
            return job;
        }

        public void Update(ObjectId id, Job jobIn) =>
            _jobs.ReplaceOne(job => job.Id == id, jobIn);

        public void Remove(ObjectId id) =>
            _jobs.DeleteOne(job => job.Id == id);
    }
}