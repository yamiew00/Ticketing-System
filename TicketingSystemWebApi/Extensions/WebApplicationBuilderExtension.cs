using MongoDB.Bson.Serialization;
using TicketingSystemModel;
using TicketingSystemWebApi.Processors;
using TicketingSystemWebApi.Services.Event;
using TicketingSystemWebApi.Services.Event.Providers;
using TicketingSystemWebApi.Services.Event.Purchase;
using TicketingSystemWebApi.Services.Login;
using TicketingSystemWebApi.Services.Register;
using TicketingSystemWebApi.Tools;

namespace TicketingSystemWebApi.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddMongoContext(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddMongoContext(new TicketSystemMongoContext(configuration.GetConnectionString("MongoDbConnection")));
            
            //add datetime serializer
            BsonSerializer.RegisterSerializer(typeof(DateTime), new TaiwanDateTimeSerializer());

            return builder;
        }

        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            //services
            services.AddScoped<RegisterService>();
            services.AddScoped<LoginService>();
            services.AddScoped<EventService>();

            //providers
            services.AddSingleton<EventDetailModelProvider>();

            //handlers
            services.AddScoped<PurchaseHandler>();

            //processors
            services.AddSingleton<CSRFValidator>();
            services.AddScoped<HttpContextManager>();
            services.AddSingleton<PaymentProcessor>();

            return services;
        }
    }
}
