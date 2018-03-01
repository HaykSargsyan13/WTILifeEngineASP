using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Infrastructure
{
    public static class MongoExtensions
    {

        /// <summary>
        /// Extension method which determines whether a collection name is in DB or not
        /// </summary>
        /// <returns></returns>
        public static bool Contains(this IMongoDatabase db, string collectionName)
        {
            List<string> collectionNames = db.CollectionNames();
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
