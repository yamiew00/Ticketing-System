using Microsoft.AspNetCore.Mvc;
using TicketingSystemWebApi.Controllers.ControllerBases;
using TicketingSystemWebApi.Services;
using TicketingSystemWebApi.Services.Login;
using TicketingSystemWebApi.Services.Login.Login;

namespace TicketingSystemWebApi.Controllers
{
    public class LoginController : NoAuthControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            this._loginService = loginService;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<ResponseBase<LoginResponse>> Login(LoginRequest request)
        {
            LoginResponse response = await _loginService.Login(request);
            return new ResponseBase<LoginResponse>(response);
        }
    }
}
