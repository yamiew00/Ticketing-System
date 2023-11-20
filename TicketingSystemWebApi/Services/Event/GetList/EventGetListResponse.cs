using TicketingSystemModel.Ticketing;

namespace TicketingSystemWebApi.Services.Event.EventList
{
    public class EventGetListResponse
    {
        public List<EventGetListResponse_Event> Events { get; set;  }
    }

    public class EventGetListResponse_Event
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public EventType EventType { get; set; }

        public int AvailableTicketCount { get; set; }
    }
}
