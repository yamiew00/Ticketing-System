﻿

using System.Text.Json.Serialization;

namespace TicketingSystemWebApi.Services
{
    public class ResponseBase<TData>
    {
        public TData Data { get; set; }

        public int ErrCode { get; set; }

        public string Msg { get; set; }

        public ResponseBase(TData data)
        {
            Data = data;
        }
    }

    public class ResponseBase
    {
        public int ErrCode { get; set; }

        public string Msg { get; set; }
    }
}
