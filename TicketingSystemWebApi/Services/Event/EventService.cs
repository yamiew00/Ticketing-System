using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoGogo.Connection;
using System.Linq.Expressions;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Services.Event.EventList;
using TicketingSystemWebApi.Services.Event.Models;

namespace TicketingSystemWebApi.Services.Event
{
    public class EventService
    {
        private readonly IGoCollection<EventEntity> _eventCollection;
        private readonly IGoCollection<TicketEntity> _ticketCollection;

        public EventService(IGoCollection<EventEntity> eventCollection,
                            IGoCollection<TicketEntity> ticketCollection)
        {
            this._eventCollection = eventCollection;
            this._ticketCollection = ticketCollection;
        }

        internal async Task<EventListResponse> EventList(EventListRequest request)
        {
            var aggregate = _eventCollection.MongoCollection.Aggregate()
                                                            .Lookup(
                                                                _ticketCollection.MongoCollection,
                                                                @event => @event.Id,
                                                                ticket => ticket.EventId,
                                                                (EventWithTickets eventWithTickets) => eventWithTickets.Ticket
                                                            )
                                                            .Unwind<EventWithTickets, EventWithOneTicket>(eventWithTickets => eventWithTickets.Ticket)
                                                            .Match(eventWithOneTicket => eventWithOneTicket.Ticket.SaleUserInfo.IsSold == false &&
                                                                                         eventWithOneTicket.Ticket.PurchaseInfo.SaleStartTime <= DateTime.UtcNow &&
                                                                                         eventWithOneTicket.Ticket.PurchaseInfo.SaleEndTime >= DateTime.UtcNow)
                                                            .Group(e => e.Id, g => new EventDetailModel
                                                            {
                                                                Id = g.Key,
                                                                Detail = g.Select(e => e.Detail).First(),
                                                                Schedule = g.Select(e => e.Schedule).First(),
                                                                Metadata = g.Select(e => e.Metadata).First(),
                                                                TicketCount = g.Count()
                                                            });

            var list = await aggregate.ToListAsync();
            return new EventListResponse();
        }
    }
}
