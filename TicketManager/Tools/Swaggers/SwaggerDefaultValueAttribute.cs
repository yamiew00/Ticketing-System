namespace TicketManager.Tools.Swaggers
{
    public class SwaggerDefaultValueAttribute : Attribute
    {
        public object Value { get; }

        public SwaggerDefaultValueAttribute(object value)
        {
            Value = value;
        }
    }
}
