using Microsoft.AspNetCore.Mvc;

namespace TicketingSystemWebApi.Controllers.ControllerBases
{
    [ApiController]
    [Route("[controller]")]
    public abstract class NoAuthControllerBase : ControllerBase
    {
    }
}
