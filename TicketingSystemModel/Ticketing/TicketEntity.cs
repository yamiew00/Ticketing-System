﻿using MongoDB.Bson.Serialization.Attributes;
using MongoGogo.Connection;

namespace TicketingSystemModel.Ticketing
{
    [MongoCollection(fromDatabase: typeof(TicketSystemMongoContext.Ticketing),
                 collectionName: "Ticket")]
    public class TicketEntity
    {
        [BsonId]
        public string TicketId { get; set; }

        /// <summary>
        /// foreign to 'EventEntity.Id'
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 購票信息
        /// </summary>
        public TicketPurchaseInfo PurchaseInfo { get; set; } = new TicketPurchaseInfo();

        /// <summary>
        /// 販賣訊息
        /// </summary>
        public TicketSaleUserInfo SaleUserInfo { get; set; } = new TicketSaleUserInfo();
    }

    /// <summary>
    /// 販賣訊息
    /// </summary>
    public class TicketSaleUserInfo
    {
        /// <summary>
        /// 買家的User.Id，未被賣出時為空值
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 是否已被賣出
        /// </summary>
        public TicketStatus IsSold { get; set; }
    }

    public enum TicketStatus
    {
        Sold = 0,            // 已售出
        PendingPurchase = 1, // 認購中
        Available = 2        // 販賣中
    }

    /// <summary>
    /// 購票信息
    /// </summary>
    public class TicketPurchaseInfo
    {
        /// <summary>
        /// 允許購買的開始時間
        /// </summary>
        public DateTime SaleStartTime { get; set; }

        /// <summary>
        /// 允許購買的結束時間
        /// </summary>
        public DateTime SaleEndTime { get; set; }
    }
}
