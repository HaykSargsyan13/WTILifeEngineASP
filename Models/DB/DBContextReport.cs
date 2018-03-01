using ASP.Infrastructure;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Models.DB
{
    public class DBContextReport
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;
        const string _dbAccounts = "Accounts";

        public DBContextReport()
        {
            string connectionString = "mongodb://localhost:27017/Report";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
        }

        private IMongoCollection<DBAccountModel> Accounts
        {
            get {return database.GetCollection<DBAccountModel>(_dbAccounts); }
        }

        /// <summary>
        /// Add <seealso cref="ReportItem"/> collection to DB 
        /// </summary>
        /// <param name="items"><seealso cref="ReportItem"/> collection </param>
        /// <param name="name">Collection name in DB</param>
        /// <returns></returns>
        public void Create(IEnumerable<ReportItem> items , string name = "ReportItems")
        {
            var Reports = database.GetCollection<ReportItem>(name);
            List<ReportItem> list = new List<ReportItem>();
            if (database.Contains(name))
            {
                foreach (var item in items)
                {
                    var filter = Builders<ReportItem>.Filter.Eq(doc => doc.AccountNo, item.AccountNo);
                    var account = Reports.Find(doc => doc.AccountNo == item.AccountNo).FirstOrDefault();
                    if (account != null)
                    {
                        item.Id = account.Id;
                        ReplaceOneResult replaceOneResult = Reports.ReplaceOne(
                            new BsonDocument("_id", new ObjectId(account.Id)), item, new UpdateOptions() { IsUpsert = true });
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
                if (list.Count > 0)
                    Reports.InsertMany(list);
            }
            else if (items.Count() > 0)
            {
                try
                {
                    Reports.InsertMany(items);
                }
                catch 
                {
                    Debug.WriteLine("Exeption");
                }
            }
        }

        /// <summary>
        /// Get list of <seealso cref="ReportItem"/> for specific account
        /// </summary>
        /// <param name="accountNo">account name</param>
        /// <returns></returns>
        public async Task<IEnumerable<ReportItem>> AccountReports(string accountNo)
        {
            List<ReportItem> list = new List<ReportItem>();
            var naems = database.CollectionNames();
            foreach (string name in naems)
            {
                if (name == _dbAccounts)
                    continue;
                var reportCollection = database.GetCollection<ReportItem>(name);
                var reportItem = await reportCollection.Find(x => x.AccountNo == accountNo).FirstOrDefaultAsync();
                if (reportItem != null)
                    list.Add(reportItem);
            }
            return list;
        }

        #region Not used yet

        private async Task AddAccountsObjects(IEnumerable<ReportItem> items, string name)
        {
            List<DBAccountModel> dbAccountModel = new List<DBAccountModel>();
            var builder = new FilterDefinitionBuilder<DBAccountModel>();
            var filter = builder.Empty;
            var accounts = await Accounts.Find(filter).ToListAsync();
            List<string> names = new List<string>();
            foreach (var account in accounts)
            {
                names.Add(account.AccountNo);
            }
            await Accounts.DeleteManyAsync(filter);
            foreach (ReportItem item in items)
            {
               if( names.Contains(item.AccountNo))
                {

                }
                    //if (!String.IsNullOrWhiteSpace(item.AccountNo))
                    //{
                    //    var filter = Builders<DBAccountModel>.Filter.Eq(doc => doc.AccountNo, item.AccountNo);
                    //    var account = Accounts.Find(doc => doc.AccountNo == item.AccountNo).FirstOrDefault();
                    //    account.CollectionId.Add(name, item.Id);
                    //    var result = Accounts.ReplaceOneAsync(doc => doc.AccountNo == account.AccountNo, account);
                    //    if (account == null)
                    //    {
                    //        dbAccountModel.Add(new DBAccountModel
                    //        {
                    //            AccountNo = item.AccountNo,
                    //            CollectionId = new Dictionary<string, string>
                    //            {
                    //              {name, item.Id }
                    //            }
                    //        });
                    //    }
                    //}

            }
            if(dbAccountModel != null && dbAccountModel.Count > 0)
            await Accounts.InsertManyAsync(dbAccountModel);
        }

        private async Task Update(DBAccountModel p)
        {
            await Accounts.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }

        #endregion
        
        /// <summary>
        /// Get reports from DB for special date
        /// </summary>
        /// <param name="date"><seealso cref="DateTime"/> in <seealso cref="string"/> format</param>
        /// <returns><seealso cref="IEnumerable{ReportItem}"/></returns>
        public async Task<IEnumerable<ReportItem>> GetReports(string date)
        {
            var Reports = database.GetCollection<ReportItem>(date);
            var builder = new FilterDefinitionBuilder<ReportItem>();
            var filter = builder.Empty;
            long count = Reports.Count(filter);
            return await Reports.Find(filter).ToListAsync();
        }
    }
}
