using TicketingSystemModel.Ticketing;

namespace TicketingSystemWebApi.Services.Event.EventList
{
    public class EventWithOneTicket : EventEntity
    {
        public TicketEntity Ticket { get; set; }
    }
}
