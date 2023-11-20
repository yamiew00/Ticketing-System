using LinqKit;
using MongoDB.Driver;
using MongoGogo.Connection;
using System.Linq.Expressions;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Services.Event.Models;

namespace TicketingSystemWebApi.Services.Event.Providers
{
    public class EventDetailModelProvider
    {
        private readonly IGoCollection<EventEntity> _eventCollection;
        private readonly IGoCollection<TicketEntity> _ticketCollection;

        public EventDetailModelProvider(IGoCollection<EventEntity> eventCollection,
                                        IGoCollection<TicketEntity> ticketCollection)
        {
            this._eventCollection = eventCollection;
            this._ticketCollection = ticketCollection;
        }

        internal async Task<List<EventDetailModel>> GetEventDetailModelByDate(DateQuery_EventDetailModel dateQuery)
        {
            //取出符合時間的events，並計算未售的票數
            var aggregate = _eventCollection.MongoCollection.Aggregate()
                                                            .Match(BuildDynamicMatchFilter(dateQuery))
                                                            .Lookup(
                                                                _ticketCollection.MongoCollection,
                                                                @event => @event.Id,
                                                                ticket => ticket.EventId,
                                                                (EventWithTickets eventWithTickets) => eventWithTickets.Ticket
                                                            )
                                                            .Unwind<EventWithTickets, EventWithOneTicket>(eventWithTickets => eventWithTickets.Ticket)
                                                            .Match(eventWithOneTicket => eventWithOneTicket.Ticket.SaleUserInfo.IsSold == TicketStatus.Available &&
                                                                                         eventWithOneTicket.Ticket.PurchaseInfo.SaleStartTime <= DateTime.UtcNow &&
                                                                                         eventWithOneTicket.Ticket.PurchaseInfo.SaleEndTime >= DateTime.UtcNow)
                                                            .Group(e => e.Id, g => new EventDetailModel
                                                            {
                                                                Id = g.Key,
                                                                Detail = g.Select(e => e.Detail).First(),
                                                                Schedule = g.Select(e => e.Schedule).First(),
                                                                Metadata = g.Select(e => e.Metadata).First(),
                                                                AvailableTicketCount = g.Count()
                                                            });
            return await aggregate.ToListAsync();
        }

        /// <summary>
        /// 根據傳值的存在性，動態組建match語法
        /// </summary>
        /// <param name="dateQuery"></param>
        /// <returns></returns>
        private static Expression<Func<EventEntity, bool>> BuildDynamicMatchFilter(DateQuery_EventDetailModel dateQuery)
        {
            var predicate = PredicateBuilder.New<EventEntity>(true);

            if (dateQuery.StartAt.HasValue)
            {
                predicate = predicate.And(@event => @event.Schedule.StartAt >= dateQuery.StartAt.Value);
            }
            if (dateQuery.EndAt.HasValue)
            {
                predicate = predicate.And(@event => @event.Schedule.EndAt <= dateQuery.EndAt.Value);
            }
            return predicate;
        }

        /// <summary>
        /// aggreation 物件1 (unwind用)
        /// </summary>
        private class EventWithTickets : EventEntity
        {
            public List<TicketEntity> Ticket { get; set; }
        }

        /// <summary>
        /// aggreation 物件2 (unwind用)
        /// </summary>
        private class EventWithOneTicket : EventEntity
        {
            public TicketEntity Ticket { get; set; }
        }
    }

    /// <summary>
    /// 可使用的query filter
    /// </summary>
    public class DateQuery_EventDetailModel
    {
        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
