namespace TicketingSystemWebApi.Exceptions
{
    public class DuplicateUserName_1100Exception : TicketingSystemExceptionBase
    {
        public DuplicateUserName_1100Exception(string msg = default) : base(msg)
        {
        }

        public override ErrorCode ErrorCode => ErrorCode.DuplicateUserName1100;
    }
}
