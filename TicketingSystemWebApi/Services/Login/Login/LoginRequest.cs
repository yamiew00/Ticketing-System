using System.ComponentModel.DataAnnotations;

namespace TicketingSystemWebApi.Services.Login.Login
{
    public class LoginRequest
    {
        [RegularExpression(@"^[a-zA-Z0-9]{6,}$", ErrorMessage = "Username must be at least 6 characters long and contain only letters and numbers.")]
        public string UserName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{8,}$", ErrorMessage = "Username must be at least 8 characters long and contain only letters and numbers.")]
        public string Password { get; set; }
    }
}
