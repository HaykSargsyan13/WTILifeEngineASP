using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System;

namespace ASP.Models
{
    public class DBAccountModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AccountNo { set; get; }
        public Dictionary<string, string> CollectionId;
    }
}
