using MongoDB.Bson.Serialization.Attributes;
using MongoGogo.Connection;

namespace TicketingSystemModel.Ticketing
{
    /// <summary>
    /// 成立的交易記錄。
    /// </summary>
    /// <remarks> 主要用途是利用這張表證明高併發購買的系統正確性</remarks>
    [MongoCollection(fromDatabase: typeof(TicketSystemMongoContext.Ticketing),
                     collectionName: "TransactionRecord")]
    public class TransactionRecordEntity
    {
        [BsonId]
        public string Id { get; set; }

        /// <summary>
        /// foreign to TicketEntity.Id
        /// </summary>
        public List<string> Tickets { get; set; }

        /// <summary>
        /// foreigh to UserEntity.Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 成交時間
        /// </summary>
        public DateTime Time { get; set; }
    }
}
