using System.Text.Json;
using TicketingSystemWebApi.Exceptions;
using TicketingSystemWebApi.Services;


namespace TicketingSystemWebApi.Controllers.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _Next;

        public GlobalExceptionMiddleware(RequestDelegate _next)
        {
            _Next = _next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (TicketingSystemExceptionBase ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;

                ResponseBase response = new ResponseBase
                {
                    ErrCode = (int)ex.ErrorCode,
                    Msg = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
            catch(Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ResponseBase response = new ResponseBase
                {
                    ErrCode = 500,
                    Msg = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }
    }
}
