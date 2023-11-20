using Microsoft.AspNetCore.Mvc;
using TicketingSystemWebApi.Controllers.ControllerBases;
using TicketingSystemWebApi.Services;
using TicketingSystemWebApi.Services.Register;
using TicketingSystemWebApi.Services.Register.Register;

namespace TicketingSystemWebApi.Controllers
{
    public class RegisterController : NoAuthControllerBase
    {
        private readonly RegisterService _registerService;

        public RegisterController(RegisterService registerService)
        {
            this._registerService = registerService;
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<ResponseBase> Register(RegisterRequest request)
        {
            await _registerService.Register(request);
            return new ResponseBase();
        }
    }
}
