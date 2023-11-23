namespace TicketingSystemWebApi.Processors
{
    /// <summary>
    /// 支付相關功能
    /// </summary>
    public class PaymentProcessor
    {
        public async Task<PaymentResponseData> Process(PaymentRequestData request)
        {
            //模擬真實情境的網路延遲
            await Task.Delay(300);

            return new PaymentResponseData
            {
                IsSuccess = true
            };
        }

        public class PaymentRequestData
        {
            public string PaymentToken { get; set; }

            /// <summary>
            /// 支付額
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// 幣種
            /// </summary>
            public string Currency { get; set; } = "USD";

            public string EventId { get; set; }
        }

        public class PaymentResponseData
        {
            public bool IsSuccess { get; set; }

            public string Message { get; set; }
        }
    }
}
