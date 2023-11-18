using TicketingSystemModel;
using TicketingSystemWebApi.Services.Register;

namespace TicketingSystemWebApi.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddMongoContext(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddMongoContext(new TicketSystemMongoContext(configuration.GetConnectionString("MongoDbConnection")));

            return builder;
        }

        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            //services
            services.AddScoped<RegisterService>();

            return services;
        }
    }
}
