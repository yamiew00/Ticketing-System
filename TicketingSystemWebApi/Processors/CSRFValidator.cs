using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;

namespace TicketingSystemWebApi.Processors
{
    public class CSRFValidator
    {
        private readonly IGoCollection<TokenEntity> _tokenCollection;

        public CSRFValidator(IGoCollection<TokenEntity> tokenCollection)
        {
            this._tokenCollection = tokenCollection;
        }

        public async Task<bool> Validate(CSRFValidatorParams @params)
        {
            //因為token collection有設ttl，所以存在的token必然是有效的
            return await _tokenCollection.CountAsync(token => token.UserId == @params.UserId && token.CSRFToken == @params.CSRFToken) > 0;
        }

        #region inner class
        public class CSRFValidatorParams
        {
            public string UserId { get; private set; }

            public string CSRFToken { get; private set; }

            public CSRFValidatorParams(string userId, string csrfToken)
            {
                UserId = userId;
                CSRFToken = csrfToken;
            }
        }
        #endregion
    }
}
