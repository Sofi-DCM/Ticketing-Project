using System.Text.Json.Serialization;

namespace Domain.Constants
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortEventsBy
    {
        Newest,
        DateAsc,
        DateDesc,
        NameAsc,
        NameDesc,
    }
}
