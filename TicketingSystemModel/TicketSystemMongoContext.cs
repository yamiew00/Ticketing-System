using MongoGogo.Connection;

namespace TicketingSystemModel
{
    public class TicketSystemMongoContext : GoContext<TicketSystemMongoContext>
    {
        [MongoDatabase(dbName: "Ticketing")]
        public class Ticketing { }

        public TicketSystemMongoContext(string connectionString) : base(connectionString)
        {
        }
    }
}
