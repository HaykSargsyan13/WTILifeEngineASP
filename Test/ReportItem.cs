//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using System.Collections.Generic;

//namespace TestApp
//{
//    public class ReportItem
//    {
//        [BsonRepresentation(BsonType.ObjectId)]
//        public string Id { get; set; }
//        public string AccountNo;
//        public decimal TotalMarketValue;
//        public decimal Cash;
//        public decimal PercentCash;
//        public Dictionary<string, decimal> Quantity;
//        public Dictionary<string, decimal> MarketValue;
//        public Dictionary<string, decimal> Percent;
//        public decimal TargetCashAlloc;
//        public decimal TargetRiskScoreUS;
//        public decimal TargetRiskScoreIntl;
//        public decimal TargetIntlAlloc;
//        public decimal ActualCashAlloc;
//        public decimal ActualRiskScoreUS;
//        public decimal ActualRiskScoreIntl;
//        public decimal ActualIntlAlloc;
//        public bool NeedsRebalance;
//        public bool NeedsCashRebalance;
//    }
//}
