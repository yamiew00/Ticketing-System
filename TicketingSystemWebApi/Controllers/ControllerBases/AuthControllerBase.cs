using Microsoft.AspNetCore.Mvc;
using TicketingSystemWebApi.Controllers.Middlewares;
using TicketingSystemWebApi.Controllers.Pipelines;

namespace TicketingSystemWebApi.Controllers.ControllerBases
{
    /// <summary>
    /// 需驗證的路由
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [MiddlewareFilter(typeof(AuthPipeline))]
    public class AuthControllerBase : ControllerBase
    {
        protected CurrentUser CurrentUser => (HttpContext.Items[AuthMiddleware.USER_ITEM_KEY] as CurrentUser)!;
    }

    public class CurrentUser
    {
        public string UserId { get; set; }

        public string FullName { get; set; }
    }
}
