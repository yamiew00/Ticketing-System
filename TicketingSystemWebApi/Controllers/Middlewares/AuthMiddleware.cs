using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketingSystemWebApi.Controllers.ControllerBases;
using TicketingSystemWebApi.Exceptions;
using TicketingSystemWebApi.Processors;

namespace TicketingSystemWebApi.Controllers.Middlewares
{
    /// <summary>
    /// 驗證中介。
    /// 在context.Items中傳CurrentUser。
    /// </summary>
    public class AuthMiddleware
    {
        public static readonly string USER_ITEM_KEY = "user";

        protected readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var httpContextManager = context.RequestServices.GetService<HttpContextManager>();
            var userModel = await httpContextManager.GetUserModelFromHeader(context) ?? throw new InvalidIdentityException();

            context.Items.Add(USER_ITEM_KEY, 
            new CurrentUser
            {
                UserId = userModel.UserId,
                FullName = userModel.FullName
            });

            await _next(context);
        }
    }
}
