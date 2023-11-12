using PortalLegisAmbiental.Domain.Enums;
using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class PermissaoResponse
    {
        public ulong Id { get; set; }
        public string Recurso { get; set; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EScopeType Scope { get; set; }
    }
}
