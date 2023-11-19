using Microsoft.AspNetCore.Mvc;
using TicketManager.Controllers.ControllerBases;
using TicketManager.Services;
using TicketManager.Services.Event;
using TicketManager.Services.Event.GenerateRandom;

namespace TicketManager.Controllers
{
    public class EventController : NoAuthControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            this._eventService = eventService;
        }

        /// <summary>
        /// 生成隨機活動和相關票券。
        /// 此API會隨機生成指定數量的活動（Event）和每個活動的票券（Ticket）。
        /// 預設情況下，會生成2個活動：一個演唱會（Concert）和一個展覽（Exhibition），
        /// 每個活動預設生成1000張票券。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GenerateRandom")]
        public async Task<ResponseBase<GenerateRandomResponse>> GenerateRandom(GenerateRandomRequest request)
        {
            GenerateRandomResponse response = await _eventService.GenerateRandom(request);
            return new ResponseBase<GenerateRandomResponse>(response);
        }
    }
}
