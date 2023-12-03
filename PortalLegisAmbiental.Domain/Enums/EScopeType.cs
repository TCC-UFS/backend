using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EScopeType
    {
        All,
        Read,
        Write,
        Delete
    }
}
