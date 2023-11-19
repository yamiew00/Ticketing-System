using TicketManager.Tools.Swaggers;

namespace TicketManager.Services.Event.GenerateRandom
{
    public class GenerateRandomRequest
    {
        [SwaggerDefaultValue(2)]
        public int GenerateEventCount { get; set; } = 2;

        [SwaggerDefaultValue(3500)]
        public int GenerateTicketCountPerEvent { get; set; } = 3500;
    }
}
