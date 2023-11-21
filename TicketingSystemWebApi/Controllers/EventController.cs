using Microsoft.AspNetCore.Mvc;
using TicketingSystemWebApi.Controllers.ControllerBases;
using TicketingSystemWebApi.Controllers.Pipelines;
using TicketingSystemWebApi.Services;
using TicketingSystemWebApi.Services.Event;
using TicketingSystemWebApi.Services.Event.EventList;
using TicketingSystemWebApi.Services.Event.Purchase;

namespace TicketingSystemWebApi.Controllers
{
    public class EventController : AuthControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            this._eventService = eventService;
        }

        /// <summary>
        /// 取得event的清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseBase<EventGetListResponse>> GetList(EventGetListRequest request)
        {
            EventGetListResponse response = await _eventService.GetList(request);
            return new ResponseBase<EventGetListResponse>(response);
        }

        /// <summary>
        /// 購票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Purchase")]
        [MiddlewareFilter(typeof(CSRFValidatePipeline))]
        public async Task<ResponseBase<EventPurchaseResponse>> Purchase(EventPurchaseRequest request)
        {
            EventPurchaseResponse response = await _eventService.Purchase(request);
            return new ResponseBase<EventPurchaseResponse>(response);   
        }
    }
}
