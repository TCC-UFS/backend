using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EAmbitoType
    {
        Estadual,
        Federal
    }
}
