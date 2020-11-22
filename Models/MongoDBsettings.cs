using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerInfo.Models
{
    public class MongoDBsettings : IMongoDBsettings
    {
        public string JobsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDBsettings
    {
        string JobsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
