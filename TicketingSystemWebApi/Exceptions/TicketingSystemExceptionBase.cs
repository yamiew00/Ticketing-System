namespace TicketingSystemWebApi.Exceptions
{
    public abstract class TicketingSystemExceptionBase : Exception
    {
        public abstract ErrorCode ErrorCode { get;}

        public TicketingSystemExceptionBase(string msg = default): base(msg) 
        {

        }
    }
}
