using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using System.Configuration;
using MongoDB.Driver;

namespace Ent.DataAccess
{
    public class MongoContext
    {
        public static  IMongoDatabase Db
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["mongoDbConfig"].ConnectionString;
                if (String.IsNullOrWhiteSpace(connectionString)) connectionString = "Server=127.0.0.1:27017;MinimumPoolSize=10;MaximumPoolSize=500;Pooled=true";


                MongoClient client = new MongoClient(connectionString);


                IMongoDatabase database = client.GetDatabase("FitMent");


                return database;
            }
        }
    }
}
