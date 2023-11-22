using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Exceptions;
using TicketingSystemWebApi.Extensions;
using TicketingSystemWebApi.Services.Login.Login;

namespace TicketingSystemWebApi.Services.Login
{
    public class LoginService
    {
        private readonly IGoCollection<UserEntity> _userCollection;
        private readonly IGoCollection<TokenEntity> _tokenCollection;

        public LoginService(IGoCollection<UserEntity> userCollection,
                            IGoCollection<TokenEntity> tokenCollection)
        {
            this._userCollection = userCollection;
            this._tokenCollection = tokenCollection;
        }

        internal async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userCollection.FindOneAsync(filter: u => u.Account.UserName == request.UserName &&
                                                                       u.Account.Password == request.Password,
                                                          projection: projecter => projecter.Include(u => u.Id));

            if (user == null) throw new InvalidIdentity_1101Exception("帳號或密碼錯誤!");

            TokenEntity resultToken = await GenerateOrProlongToken(user.Id);

            return new LoginResponse
            {
                LoginToken = resultToken.LoginToken,
                CSRFToken = resultToken.CSRFToken,
                ExpireAt = resultToken.ExpireAt,
            };
        }

        private async Task<TokenEntity> GenerateOrProlongToken(string userId)
        {
            var resultToken = await _tokenCollection.FindOneAsync(token => token.UserId == userId);

            if (resultToken != null &&
               resultToken.ExpireAt <= DateTime.UtcNow.AddHours(1))
            {
                //ProlongTokenLifetime
                resultToken.ExpireAt = DateTime.UtcNow.AddHours(3);

                await _tokenCollection.UpdateOneAsync(token => token.UserId == resultToken.UserId,
                                                      updater => updater.Set(token => token.ExpireAt, resultToken.ExpireAt));
            }
            else if(resultToken == null)
            {
                resultToken = new TokenEntity
                {
                    ExpireAt = DateTime.UtcNow.AddHours(3),
                    CSRFToken = Guid.NewGuid().ToString(),
                    LoginToken = Guid.NewGuid().ToString(),
                    UserId = userId
                };

                await _tokenCollection.ReplaceOneAsync(filter: token => token.UserId == userId,
                                                       document: resultToken,
                                                       isUpsert: true);
            }

            return resultToken;
        }
    }
}
