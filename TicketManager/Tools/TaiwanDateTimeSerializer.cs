using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

namespace TicketManager.Tools
{
    /// <summary>
    /// 將utc轉為臺灣時間
    /// </summary>
    public class TaiwanDateTimeSerializer : DateTimeSerializer
    {
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var utcDateTime = base.Deserialize(context, args);
            return ConvertUtcToTaiwanTime(utcDateTime);
        }

        private DateTime ConvertUtcToTaiwanTime(DateTime utcDateTime)
        {
            try
            {
                TimeZoneInfo taipeiZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, taipeiZone);
            }
            catch
            {
                // Handle exceptions or default to UTC if timezone not found
                return utcDateTime;
            }
        }
    }
}
