using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp;

namespace ASP.Models.DB
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

        /// <summary>
        /// Add <seealso cref="ReportItem"/> collection to DB 
        /// </summary>
        /// <param name="items"><seealso cref="ReportItem"/> collection </param>
        /// <param name="name">Collection name in DB</param>
        /// <returns></returns>
        public async Task Create(IEnumerable<ReportItem> items , string name = "ReportItems")
        {
            var Reports = database.GetCollection<ReportItem>(name);
            await Reports.InsertManyAsync(items);
        }

        /// <summary>
        /// Get reports from DB for special date
        /// </summary>
        /// <param name="date"><seealso cref="DateTime"/> in <seealso cref="string"/> format</param>
        /// <returns><seealso cref="IEnumerable{ReportItem}"/></returns>
        public async Task<IEnumerable<ReportItem>> GetReports(string date/*, FilterDefinition<ReportItem> filter=null*/)
        {
            var Reports = database.GetCollection<ReportItem>(date);
            var builder = new FilterDefinitionBuilder<ReportItem>();
            var filter = builder.Empty;
            return await Reports.Find(filter).ToListAsync();
        }
    }
}
