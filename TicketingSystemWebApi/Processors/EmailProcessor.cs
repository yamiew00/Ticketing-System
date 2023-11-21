namespace TicketingSystemWebApi.Processors
{
    public class EmailProcessor
    {
        public async Task SendPurchaseSuccessEmail(string userId, List<string> ticketIds)
        {
            //模擬背後的寄信系統。假設是有mq機制可以列隊。
            await Task.Delay(100);
        }
    }
}
