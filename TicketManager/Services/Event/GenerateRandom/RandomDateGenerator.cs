namespace TicketManager.Services.Event.GenerateRandom
{
    public class RandomDateGenerator
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// 生成隨機日期時間
        /// 從現在開始算起的未來一個月到六個月之間
        /// 時間隨機落在早上 8 點到晚上 7 點之間，分和秒均為 0
        /// </summary>
        /// <returns>隨機日期時間</returns>
        public static DateTime GenerateRandomDateTime()
        {
            DateTime now = DateTime.UtcNow;

            // 生成隨機天數，範圍是未來一個月到六個月之內
            int daysToAdd = _random.Next(30, 180); // 30 天到 180 天

            // 生成隨機小時數，範圍是 8 點到 19 點
            int hours = _random.Next(8, 20); // 8 到 19

            // 新的日期和時間
            return now.Date.AddDays(daysToAdd).AddHours(hours);
        }
    }
}
