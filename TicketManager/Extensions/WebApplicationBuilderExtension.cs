using MongoDB.Bson.Serialization;
using TicketingSystemModel;
using TicketManager.Services.Event;
using TicketManager.Tools;

namespace TicketManager.Extensions
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
            services.AddScoped<EventService>();
            return services;
        }
    }
}
