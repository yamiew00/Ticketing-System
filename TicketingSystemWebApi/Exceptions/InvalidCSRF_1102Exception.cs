namespace TicketingSystemWebApi.Exceptions
{
    public class InvalidCSRF_1102Exception : TicketingSystemExceptionBase
    {
        public InvalidCSRF_1102Exception(string? msg = default) : base(msg)
        {
        }

        public override ErrorCode ErrorCode => ErrorCode.InvalidCSRF1102;
    }
}
