using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace TicketManager.Tools.Swaggers
{
    public class SwaggerDefaultValues : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var defaultValues = context.Type.GetProperties()
                .Where(p => p.GetCustomAttributes<SwaggerDefaultValueAttribute>().Any())
                .ToDictionary(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1), p => p.GetCustomAttribute<SwaggerDefaultValueAttribute>().Value);

            foreach (var entry in schema.Properties)
            {
                if (defaultValues.TryGetValue(entry.Key, out var value))
                {
                    entry.Value.Default = new OpenApiString(value.ToString());
                }
            }
        }
    }
}
