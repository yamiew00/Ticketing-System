using TicketingSystemModel.Ticketing;

namespace TicketingSystemWebApi.Services.Event.EventList
{
    public class EventWithTickets : EventEntity
    {
        public List<TicketEntity> Ticket { get; set; }
    }
}
