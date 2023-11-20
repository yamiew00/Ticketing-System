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

        [HttpPost("GetList")]
        public async Task<ResponseBase<EventGetListResponse>> GetList(EventGetListRequest request)
        {
            EventGetListResponse response = await _eventService.GetList(request);
            return new ResponseBase<EventGetListResponse>(response);
        }
    }
}
