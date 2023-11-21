using System.Net;
using TicketingSystemWebApi.Exceptions;
using TicketingSystemWebApi.Processors;

namespace TicketingSystemWebApi.Controllers.Middlewares
{
    public class CSRFValidateMiddleware
    {
        protected readonly RequestDelegate _next;
        private readonly CSRFValidator _csrfValidator;

        public CSRFValidateMiddleware(RequestDelegate next,
                                      CSRFValidator csrfValidator)
        {
            _next = next;
            this._csrfValidator = csrfValidator;
        }

        public async Task Invoke(HttpContext context)
        {
            var httpContextManager = context.RequestServices.GetRequiredService<HttpContextManager>();

            var userModel = await httpContextManager.GetUserModelFromHeader(context) ?? throw new InvalidIdentity_1101Exception();
            var csrfToken = httpContextManager.GetCSRFTokenFromHeader(context) ?? throw new InvalidCSRF_1102Exception();

            if (!await _csrfValidator.Validate(new CSRFValidator.CSRFValidatorParams(userId: userModel.UserId, csrfToken: csrfToken))) throw new InvalidCSRF_1102Exception();
            
            await _next(context);
        }
    }
}
