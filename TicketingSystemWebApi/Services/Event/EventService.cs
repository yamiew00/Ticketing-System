using MongoDB.Driver;
using TicketingSystemWebApi.Services.Event.EventList;
using TicketingSystemWebApi.Services.Event.Models;
using TicketingSystemWebApi.Services.Event.Providers;
using TicketingSystemWebApi.Services.Event.Purchase;

namespace TicketingSystemWebApi.Services.Event
{
    public class EventService
    {
        private readonly EventDetailModelProvider _eventDetailModelProvider;
        private readonly PurchaseHandler _purchaseHandler;

        public EventService(EventDetailModelProvider eventDetailModelProvider,
                            PurchaseHandler purchaseHandler)
        {
            this._eventDetailModelProvider = eventDetailModelProvider;
            this._purchaseHandler = purchaseHandler;
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

        internal Task<EventPurchaseResponse> Purchase(EventPurchaseRequest request)
        {
            return _purchaseHandler.Purchase(request);
        }
    }
}
