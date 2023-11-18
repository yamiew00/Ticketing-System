using MongoGogo.Connection;

namespace TicketingSystemModel
{
    public class TicketSystemMongoContext : GoContext<TicketSystemMongoContext>
    {
        [MongoDatabase]
        public class Ticketing { }

        public TicketSystemMongoContext(string connectionString) : base(connectionString)
        {
        }
    }
}
