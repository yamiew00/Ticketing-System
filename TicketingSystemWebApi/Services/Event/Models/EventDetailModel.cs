using TicketingSystemModel.Ticketing;

namespace TicketingSystemWebApi.Services.Event.Models
{
    public class EventDetailModel : EventEntity
    {
        public int AvailableTicketCount { get; set; }
    }
}
