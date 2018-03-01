using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;
using ASP.Models.ViewModels;
using Microsoft.Extensions.Configuration;

namespace ASP.Models.DB
{
    public class DBContext
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;

        public DBContext()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string connectionString = configuration["ConnectionStrings:UserConnection"];
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
        }

        private IMongoCollection<LoginViewModel> Accounts
        {
            get { return database.GetCollection<LoginViewModel>("Users"); }
        }

        /// <summary>
        /// Get Account from Db if User name and password is valid
        /// </summary>
        /// <param name="name">User Name</param>
        /// <param name="password">User Password</param>
        /// <returns>Returns <seealso cref="LoginViewModel"/></returns>
        public LoginViewModel GetAccount(string name, string password)
        {
            var builder = new FilterDefinitionBuilder<LoginViewModel>();
            var filter = builder.Empty;
            // фильтр по имени
            if (!String.IsNullOrWhiteSpace(name))
            {
                filter = filter & builder.Eq("Name", name);
            }
            if (!String.IsNullOrWhiteSpace(password))
            {
                filter = filter & builder.Eq("Password", password);
            }
            var account = Accounts.Find(filter).FirstOrDefault();

            return account != null ? account : new LoginViewModel { Name = "null", Password = "null" };

        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public async Task<bool> Create(LoginViewModel c)
        {
            var builder = new FilterDefinitionBuilder<LoginViewModel>();
            var filter = builder.Empty;
            if (!String.IsNullOrWhiteSpace(c.Name))
            {
                filter = filter & builder.Eq("Name", c.Name);
            }
            LoginViewModel temp = Accounts.Find(filter).FirstOrDefault();
            if (String.IsNullOrEmpty(temp?.Name))
            {
                await Accounts.InsertOneAsync(c);
                return true;
            }
            return false;
        }

        //private async Task<IEnumerable<LoginViewModel>> GetAccounts(string userName, string password)
        //{
        //    // строитель фильтров
        //    var builder = new FilterDefinitionBuilder<LoginViewModel>();
        //    var filter = builder.Empty;
        //    if (!String.IsNullOrWhiteSpace(userName))
        //    {
        //        filter = filter & builder.Eq("Name", userName);
        //    }
        //    if (!String.IsNullOrWhiteSpace(password))
        //    {
        //        filter = filter & builder.Eq("Password", password);
        //    }

        //    return await Accounts.Find(filter).ToListAsync();
        //}

        //private async Task User(string id, string Name, string Password)
        //{
        //    await Accounts.FindAsync(new BsonDocument("_id", new ObjectId(id)));
        //}



        //private async Task Update(LoginViewModel c)
        //{
        //    await Accounts.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(c.Id)), c);
        //}

        //private async Task Remove(string id)
        //{
        //    await Accounts.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        //}


    }
}