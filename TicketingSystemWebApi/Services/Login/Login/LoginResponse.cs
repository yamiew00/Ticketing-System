namespace TicketingSystemWebApi.Services.Login.Login
{
    public class LoginResponse
    {
        public string LoginToken { get; set; }

        public string CSRFToken { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
