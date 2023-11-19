using Microsoft.AspNetCore.Mvc;
using TicketingSystemWebApi.Controllers.ControllerBases;
using TicketingSystemWebApi.Services;
using TicketingSystemWebApi.Services.Event;
using TicketingSystemWebApi.Services.Event.EventList;

namespace TicketingSystemWebApi.Controllers
{
    public class EventController : AuthControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            this._eventService = eventService;
        }

        [HttpPost("EventList")]
        public async Task<ResponseBase<EventListResponse>> EventList(EventListRequest request)
        {
            EventListResponse response = await _eventService.EventList(request);
            return new ResponseBase<EventListResponse>(response);
        }
    }
}
