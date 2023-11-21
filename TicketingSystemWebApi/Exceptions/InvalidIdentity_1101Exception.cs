namespace TicketingSystemWebApi.Exceptions
{
    public class InvalidIdentity_1101Exception : TicketingSystemExceptionBase
    {
        public InvalidIdentity_1101Exception(string msg = default) : base(msg)
        {
        }

        public override ErrorCode ErrorCode => ErrorCode.InvalidIdentity1101;
    }
}
