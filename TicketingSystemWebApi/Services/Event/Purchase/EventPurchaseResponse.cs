namespace TicketingSystemWebApi.Services.Event.Purchase
{
    public class EventPurchaseResponse
    {
        /// <summary>
        /// 購票狀態
        /// </summary>
        public EventPurchaseStatus Status { get; set; }

        public List<string> Tickets { get; set; } = new List<string>();
    }

    public enum EventPurchaseStatus
    {
        PurchaseSuccessful = 0, // 購票成功
        PaymentFailed = 1,      // 支付失敗
        InsufficientTickets = 2 // 可用的票量不足
    }
}
