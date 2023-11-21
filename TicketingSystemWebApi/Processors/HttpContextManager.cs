using MongoDB.Driver;
using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Exceptions;

namespace TicketingSystemWebApi.Processors
{
    /// <summary>
    /// 專門專門http協定的值解析。
    /// Its liftime should be scoped。
    /// </summary>
    public class HttpContextManager
    {
        private static readonly string AUTHORIZATION_HEADER_KEY = "Authorization";
        private static readonly string CSRF_HEADER_KEY = "CSRF-Token";

        /// <summary>
        /// usermodel的快取
        /// </summary>
        private UserModel _userModel;

        private readonly IGoCollection<TokenEntity> _tokenCollection;
        private readonly IGoCollection<UserEntity> _userCollection;

        public HttpContextManager(IGoCollection<TokenEntity> tokenCollection,
                                  IGoCollection<UserEntity> userCollection)
        {
            this._tokenCollection = tokenCollection;
            this._userCollection = userCollection;
        }

        public async Task<UserModel?> GetUserModelFromHeader(HttpContext context)
        {
            if(_userModel == null)
            {
                string authToken = GetAuthTokenFromHeader(context) ?? throw new InvalidIdentity_1101Exception();
                var dbToken = await _tokenCollection.FindOneAsync(filter: token => token.LoginToken == authToken,
                                                                  projection: projecter => projecter.Include(token => token.UserId));
                if (dbToken == null) return null;

                var dbUser = await _userCollection.FindOneAsync(filter: user => user.Id == dbToken.UserId,
                                                                projection: projecter => projecter.Include(user => user.PersonalInfo.FullName));
                if(dbUser == null) return null;

                _userModel = new UserModel(userId: dbToken.UserId,
                                           fullName: dbUser.PersonalInfo.FullName);
            }

            return _userModel;
        }

        public string? GetAuthTokenFromHeader(HttpContext context)
        {
            return context.Request.Headers.TryGetValue(AUTHORIZATION_HEADER_KEY, out var value) ? value.ToString() : null;
        }

        public string? GetCSRFTokenFromHeader(HttpContext context)
        {
            return context.Request.Headers.TryGetValue(CSRF_HEADER_KEY, out var value) ? value.ToString() : null;
        }

        public class UserModel
        {
            public string UserId { get; private set; }

            public string FullName { get; private set; }

            public UserModel(string userId, string fullName)
            {
                UserId = userId;
                FullName = fullName;
            }
        }
    }
}
