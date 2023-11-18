using MongoGogo.Connection;

namespace TicketingSystemModel.Ticketing
{
    [MongoCollection(fromDatabase: typeof(TicketSystemMongoContext.Ticketing),
                     collectionName: "Token")]
    public class TokenEntity : GoDocument
    {
        /// <summary>
        /// unique
        /// </summary>
        public string UserId { get; set; }

        public string LoginToken { get; set; }

        public string CSRFToken { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
