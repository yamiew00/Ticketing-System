namespace TicketManager.Services.Event.GenerateRandom
{
    public class GenerateRandomRequest
    {
        public int GenerateEventCount { get; set; } = 2;

        public int GenerateTicketCountPerEvent { get; set; } = 3500;
    }
}
