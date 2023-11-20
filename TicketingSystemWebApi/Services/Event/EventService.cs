using MongoDB.Driver;
using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Services.Event.EventList;
using TicketingSystemWebApi.Services.Event.Models;
using TicketingSystemWebApi.Services.Event.Providers;

namespace TicketingSystemWebApi.Services.Event
{
    public class EventService
    {
        private readonly IGoCollection<EventEntity> _eventCollection;
        private readonly IGoCollection<TicketEntity> _ticketCollection;
        private readonly EventDetailModelProvider _eventDetailModelProvider;

        public EventService(IGoCollection<EventEntity> eventCollection,
                            IGoCollection<TicketEntity> ticketCollection,
                            EventDetailModelProvider eventDetailModelProvider)
        {
            this._eventCollection = eventCollection;
            this._ticketCollection = ticketCollection;
            this._eventDetailModelProvider = eventDetailModelProvider;
        }

        internal async Task<EventGetListResponse> GetList(EventGetListRequest request)
        {
            List<EventDetailModel> eventDetails = await _eventDetailModelProvider.GetEventDetailModelByDate(new DateQuery_EventDetailModel { StartAt = request.StartAt, EndAt = request.EndAt});

            return new EventGetListResponse
            {
                Events = eventDetails.Select(detail => new EventGetListResponse_Event
                {
                    Id = detail.Id,
                    Description = detail.Detail.Description,
                    Title = detail.Detail.Title,
                    StartAt = detail.Schedule.StartAt,
                    EndAt = detail.Schedule.EndAt,  
                    EventType = detail.Metadata.Type,
                    AvailableTicketCount = detail.AvailableTicketCount
                }).OrderBy(detail => detail.StartAt).ToList()
            };
        }
    }
}
