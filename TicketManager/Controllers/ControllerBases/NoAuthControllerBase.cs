using Microsoft.AspNetCore.Mvc;

namespace TicketManager.Controllers.ControllerBases
{
    [ApiController]
    [Route("[controller]")]
    public class NoAuthControllerBase : ControllerBase
    {
    }
}
