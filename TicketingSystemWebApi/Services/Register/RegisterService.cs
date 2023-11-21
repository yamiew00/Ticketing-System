using MongoDB.Driver;
using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Exceptions;
using TicketingSystemWebApi.Services.Register.Register;

namespace TicketingSystemWebApi.Services.Register
{
    public class RegisterService
    {
        private readonly IGoCollection<UserEntity> _userCollection;

        public RegisterService(IGoCollection<UserEntity> userCollection)
        {
            this._userCollection = userCollection;
        }

        internal async Task Register(RegisterRequest request)
        {
            //check existence. 
            // TODO: Implement a separate API for username availability check. 
            // This will ensure the username can be securely reserved for a brief period during registration, 
            // providing a better user experience and reducing the chances of username conflicts.
            if (await _userCollection.CountAsync(user => user.Account.UserName == request.UserName) > 0) throw new DuplicateUserName_1100Exception("username exists");

            UserEntity userEntity = CreateNewUser(request);
            try
            {
                await _userCollection.InsertOneAsync(userEntity);
            }
            catch (MongoWriteException ex)
            {
                //can 
                throw new DuplicateUserName_1100Exception("username exists");
            }
        }

        private UserEntity CreateNewUser(RegisterRequest request)
        {
            return new UserEntity
            {
                Account = new Account
                {
                    Password = request.Password,
                    UserName = request.UserName,
                },
                Id = Guid.NewGuid().ToString(),
                PersonalInfo = new PersonalInfo
                {
                    FullName = request.FullName,
                    //只是為了模擬，所以不做信箱驗證
                    Email = request.Email,
                },
                PersonalActivity = new PersonalActivity
                {
                    RegistrationTime = DateTime.UtcNow
                }
            };
        }
    }
}
