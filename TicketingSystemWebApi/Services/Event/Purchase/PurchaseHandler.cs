using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Processors;

namespace TicketingSystemWebApi.Services.Event.Purchase
{
    public class PurchaseHandler
    {
        private readonly IGoCollection<TicketEntity> _ticketCollection;
        private readonly PaymentProcessor _paymentProcessor;
        private readonly EmailProcessor _emailProcessor;
        private readonly IGoCollection<TransactionRecordEntity> _transactionRecordCollection;

        public PurchaseHandler(IGoCollection<TicketEntity> ticketCollection,
                               PaymentProcessor paymentProcessor,
                               EmailProcessor emailProcessor,
                               IGoCollection<TransactionRecordEntity> transactionRecordCollection)
        {
            this._ticketCollection = ticketCollection;
            this._paymentProcessor = paymentProcessor;
            this._emailProcessor = emailProcessor;
            this._transactionRecordCollection = transactionRecordCollection;
        }

        internal async Task<EventPurchaseResponse> Purchase(EventPurchaseRequest request, 
                                                            Controllers.ControllerBases.CurrentUser currentUser)
        {
            var pendingPurchaseTickets = new List<TicketEntity>();
            var utcNow = DateTime.UtcNow;   

            for (int loopIndex = 0; loopIndex < request.TicketQuantity; loopIndex++)
            {
                // ?
                var pendingPurchaseTicket = await _ticketCollection.UpdateOneAndRetrieveAsync(filter: ticket => ticket.EventId == request.EventId &&
                                                                                                           ticket.PurchaseInfo.SaleStartTime <= utcNow &&
                                                                                                           ticket.PurchaseInfo.SaleEndTime >= utcNow &&
                                                                                                           ticket.SaleUserInfo.IsSold == TicketStatus.Available,
                                                                                              updateDefinitionBuilder: updater => updater.Set(ticket => ticket.SaleUserInfo.IsSold, TicketStatus.PendingPurchase)
                                                                                                                                         .Set(ticket => ticket.SaleUserInfo.UserId, currentUser.UserId),
                                                                                              new MongoGogo.Connection.Builders.Updates.GoUpdateOneAndRetrieveOptions<TicketEntity>
                                                                                              {
                                                                                                   ReturnDocument = MongoDB.Driver.ReturnDocument.After,
                                                                                                   IsUpsert = false,
                                                                                                   Projection = projecter => projecter.Include(ticket => ticket.TicketId)
                                                                                              });

                if (pendingPurchaseTicket != null) pendingPurchaseTickets.Add(pendingPurchaseTicket);
                else 
                {
                    await ResetTicketStatus(pendingPurchaseTickets);
                    return new EventPurchaseResponse
                    {
                        Status = EventPurchaseStatus.InsufficientTickets,
                    };
                }
            }

            //cash card
            var paymentResponse = await _paymentProcessor.Process(new PaymentProcessor.PaymentRequestData
            {
                //模擬情境，假設一張票總是10美元
                Amount = request.TicketQuantity * 10,
                EventId = request.EventId,
                PaymentToken = request.PaymentToken
            });

            if (!paymentResponse.IsSuccess)
            {
                await ResetTicketStatus(pendingPurchaseTickets);
                return new EventPurchaseResponse
                {
                    Status = EventPurchaseStatus.PaymentFailed
                };
            }
            var ticketIds = pendingPurchaseTickets.Select(ticket => ticket.TicketId).ToList();

            //email
            await _emailProcessor.SendPurchaseSuccessEmail(currentUser.UserId, ticketIds);

            //票據標記為已出售
            await MarkTicketasSoldout(pendingPurchaseTickets);

            //存下交易記錄
            TransactionRecordEntity transactionRecordEntity = new TransactionRecordEntity
            {
                Time = utcNow,
                Id = Guid.NewGuid().ToString(),
                Tickets = ticketIds,
                UserId = currentUser.UserId
            };
            await _transactionRecordCollection.InsertOneAsync(transactionRecordEntity);

            return new EventPurchaseResponse
            {
                Status = EventPurchaseStatus.PurchaseSuccessful,
                Tickets = ticketIds
            };
        }

        /// <summary>
        /// 交易失敗則讓票卷回歸可販賣狀態
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private async Task ResetTicketStatus(List<TicketEntity> tickets)
        {
            if (!tickets.Any()) return;
            var bulker = _ticketCollection.NewBulker();
            foreach (var ticket in tickets)
            {
                bulker.UpdateOne(filter: t => t.TicketId == ticket.TicketId,
                                 updateDefinitionBuilder: updater => updater.Set(t => t.SaleUserInfo.IsSold, TicketStatus.Available)
                                                                            .Set(t => t.SaleUserInfo.UserId, null));
            }

            await bulker.SaveChangesAsync();    
        }

        /// <summary>
        /// 交易成功後需讓票卷標記為已售出
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private async Task MarkTicketasSoldout(List<TicketEntity> tickets)
        {
            if (!tickets.Any()) return;
            var bulker = _ticketCollection.NewBulker();
            foreach (var ticket in tickets)
            {
                bulker.UpdateOne(filter: t => t.TicketId == ticket.TicketId,
                                 updateDefinitionBuilder: updater => updater.Set(t => t.SaleUserInfo.IsSold, TicketStatus.Sold));
            }

            await bulker.SaveChangesAsync();
        }
    }
}
