using System.ComponentModel.DataAnnotations;

namespace TicketingSystemWebApi.Services.Register.Register
{
    public class RegisterRequest
    {
        /// <summary>
        /// 至少六碼以上的英數字組合
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9]{6,}$", ErrorMessage = "Username must be at least 6 characters long and contain only letters and numbers.")]
        public string UserName { get; set; }

        /// <summary>
        /// 至少八碼以上的英數字組合
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain only letters and numbers.")]
        public string Password { get; set; }

        public string FullName { get; set; }
    }
}
