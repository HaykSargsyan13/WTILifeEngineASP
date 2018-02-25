using MongoDB.Bson;
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
        public async Task Create(IEnumerable<ReportItem> items , string name = "ReportItems")
        {
            int i = 0;
            while (database.Contains(name))
            {
                string temp;
                if (name.Contains('('))
                    temp = name.Substring(0, name.IndexOf('('));
                else
                    temp = name;
                i++;
                temp += $" ({i})";
                name = temp;
            }
            var Reports = database.GetCollection<ReportItem>(name);
            await Reports.InsertManyAsync(items);
            //await AddAccountsObjects(items, name);
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
                var reportItem = await reportCollection.Find(x => x.AccountNo == accountNo).FirstAsync();
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
        public async Task<IEnumerable<ReportItem>> GetReports(string date/*, FilterDefinition<ReportItem> filter=null*/)
        {
            var Reports = database.GetCollection<ReportItem>(date);
            var builder = new FilterDefinitionBuilder<ReportItem>();
            var filter = builder.Empty;
            return await Reports.Find(filter).ToListAsync();
        }
    }

    public static class Extension
    {
        /// <summary>
        /// Extension method which determines whether a collection name is in DB or not
        /// </summary>
        /// <returns></returns>
        public static bool Contains(this IMongoDatabase db, string collectionName)
        {
            List<string> collectionNames = new List<string>();

            foreach (BsonDocument collection in db.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
            {
                string name = collection["name"].AsString;
                collectionNames.Add(name);
            }
            return collectionNames.Contains(collectionName);
        }

        /// <summary>
        /// Extension method which get all collection names in DB
        /// </summary>
        /// <param name="db">Collection</param>
        /// <returns></returns>
        public static List<string> CollectionNames(this IMongoDatabase db)
        {
            List<string> collectionNames = new List<string>();

            foreach (BsonDocument collection in db.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
            {
                string name = collection["name"].AsString;
                collectionNames.Add(name);
            }
            return collectionNames;
        }
    }
}
