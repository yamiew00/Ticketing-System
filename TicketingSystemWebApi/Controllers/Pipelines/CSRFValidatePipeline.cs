using TicketingSystemWebApi.Controllers.Middlewares;

namespace TicketingSystemWebApi.Controllers.Pipelines
{
    public class CSRFValidatePipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<CSRFValidateMiddleware>();
        }
    }
}
