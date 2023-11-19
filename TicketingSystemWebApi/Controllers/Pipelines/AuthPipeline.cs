using TicketingSystemWebApi.Controllers.Middlewares;

namespace TicketingSystemWebApi.Controllers.Pipelines
{
    public class AuthPipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AuthMiddleware>();
        }
    }
}
