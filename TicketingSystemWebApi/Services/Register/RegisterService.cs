using MongoDB.Driver;
using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
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
            if (await _userCollection.CountAsync(user => user.Account.UserName == request.UserName) > 0) throw new Exception("username exists");

            UserEntity userEntity = CreateNewUser(request);
            try
            {
                await _userCollection.InsertOneAsync(userEntity);
            }
            catch (MongoWriteException ex)
            {
                //can 
                throw new Exception("username exists");
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
                    FullName = request.FullName
                },
                PersonalActivity = new PersonalActivity
                {
                    RegistrationTime = DateTime.Now
                }
            };
        }
    }
}
