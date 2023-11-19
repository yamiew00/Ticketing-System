using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Controllers.ControllerBases;
using TicketingSystemWebApi.Exceptions;

namespace TicketingSystemWebApi.Controllers.Middlewares
{
    /// <summary>
    /// 驗證中介。
    /// 在context.Items中傳CurrentUser。
    /// </summary>
    public class AuthMiddleware
    {
        public static readonly string AUTHORIZATION_HEADER_KEY = "";

        protected readonly RequestDelegate _next;
        private readonly IGoCollection<TokenEntity> _tokenCollection;

        public AuthMiddleware(RequestDelegate next,
                              IGoCollection<TokenEntity> tokenCollection)
        {
            _next = next;
            this._tokenCollection = tokenCollection;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var stringValues) || 
                string.IsNullOrEmpty(stringValues.ToString())) throw new InvalidIdentityException();

            var authorizationToken = stringValues.ToString();
            var dbToken = await _tokenCollection.FindOneAsync(filter: token => token.LoginToken == authorizationToken,
                                                              projection: projecter => projecter.Include(token => token.UserId)) 
                ?? throw new InvalidIdentityException();

            context.Items.Add(AUTHORIZATION_HEADER_KEY, 
            new CurrentUser
            {
                Id = dbToken.UserId
            });

            await _next(context);
        }
    }
}
