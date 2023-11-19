using MongoDB.Bson.Serialization.Attributes;
using MongoGogo.Connection;

namespace TicketingSystemModel.Ticketing
{
    [MongoCollection(fromDatabase: typeof(TicketSystemMongoContext.Ticketing),
                     collectionName: "Event")]
    public class EventEntity
    {
        [BsonId]
        public string Id { get; set; }

        public EventDetail Detail { get; set; }

        public EventSchedule Schedule { get; set; }

        public EventMetadata Metadata { get; set; }
    }

    public class EventDetail
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class EventSchedule
    {
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }
    }

    public class EventMetadata
    {
        /// <summary>
        /// Event被創立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Event類型
        /// </summary>
        public EventType Type { get; set; }
    }

    public enum EventType
    {
        Concert = 0,  // 演唱會
        Exhibition = 1 // 展覽
    }
}
