using Microsoft.AspNetCore.Mvc;

namespace TicketingSystemWebApi.Controllers.ControllerBases
{
    /// <summary>
    /// 不需驗證的路由
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public abstract class NoAuthControllerBase : ControllerBase
    {
    }
}
