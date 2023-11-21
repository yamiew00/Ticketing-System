using System.ComponentModel.DataAnnotations;

namespace TicketingSystemWebApi.Services.Event.Purchase
{
    public class EventPurchaseRequest
    {
        public string EventId { get; set; }

        [Range(1, int.MaxValue)]
        public int TicketQuantity { get; set; }

        public string CSRFToken { get; set; }

        public string PaymentToken { get; set; }
    }
}
