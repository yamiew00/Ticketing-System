using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;

namespace TicketManager.Services.Delete
{
    public class DeleteService
    {
        private readonly IGoCollection<UserEntity> _userCollection;
        private readonly IGoCollection<EventEntity> _eventCollection;
        private readonly IGoCollection<TokenEntity> _tokenCollection;
        private readonly IGoCollection<TicketEntity> _ticketCollection;
        private readonly IGoCollection<TransactionRecordEntity> _transactionRecordCollection;

        public DeleteService(IGoCollection<UserEntity> userCollection,
                             IGoCollection<EventEntity> eventCollection,
                             IGoCollection<TokenEntity> tokenCollection,
                             IGoCollection<TicketEntity> ticketCollection,
                             IGoCollection<TransactionRecordEntity> transactionRecordCollection)
        {
            this._userCollection = userCollection;
            this._eventCollection = eventCollection;
            this._tokenCollection = tokenCollection;
            this._ticketCollection = ticketCollection;
            this._transactionRecordCollection = transactionRecordCollection;
        }

        internal async Task DeleteAll()
        {
            await _userCollection.DeleteManyAsync(_ => true);
            await _eventCollection.DeleteManyAsync(_ => true);
            await _tokenCollection.DeleteManyAsync(_ => true);
            await _ticketCollection.DeleteManyAsync(_ => true);
            await _transactionRecordCollection.DeleteManyAsync(_ => true);
        }
    }
}
