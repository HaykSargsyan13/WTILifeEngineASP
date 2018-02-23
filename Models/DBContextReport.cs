using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp;

namespace ASP.Models
{
    public class DBContextReport
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;

        public DBContextReport()
        {
            string connectionString = "mongodb://localhost:27017/Report";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
        }

        public async Task Create(IEnumerable<ReportItem> items , string name = "ReportItems")
        {
            var Reports = database.GetCollection<ReportItem>(name);
            await Reports.InsertManyAsync(items);
        }

        public async Task<IEnumerable<ReportItem>> GetReports(string date)
        {
            var Reports = database.GetCollection<ReportItem>(date);
            var builder = new FilterDefinitionBuilder<ReportItem>();
            var filter = builder.Empty;
            return await Reports.Find(filter).ToListAsync();
        }
    }
}
