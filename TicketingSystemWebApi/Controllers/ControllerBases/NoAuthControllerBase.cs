using Microsoft.AspNetCore.Mvc;

namespace TicketingSystemWebApi.Controllers.ControllerBases
{
    [ApiController]
    [Route("[controller]")]
    public class NoAuthControllerBase : ControllerBase
    {
    }
}
