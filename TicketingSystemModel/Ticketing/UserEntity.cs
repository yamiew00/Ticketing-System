using MongoDB.Bson.Serialization.Attributes;
using MongoGogo.Connection;

namespace TicketingSystemModel.Ticketing
{
    /// <summary>
    /// User
    /// </summary>
    [MongoCollection(fromDatabase: typeof(TicketSystemMongoContext.Ticketing),
                     collectionName: "User")]
    public class UserEntity
    {
        [BsonId]
        public string Id { get; set; }

        public Account Account { get; set; }

        public PersonalInfo PersonalInfo { get; set; }

        public PersonalActivity PersonalActivity { get; set; }
    }

    public class Account
    {
        /// <summary>
        /// unique
        /// </summary>
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class PersonalInfo
    {
        public string FullName { get; set; }
    }

    public class PersonalActivity
    {
        public DateTime RegistrationTime { get; set; }

        public DateTime? LastLoginTime { get; set; }
    }
}
