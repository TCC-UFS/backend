using PortalLegisAmbiental.Domain.Enums;
using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class JurisdicaoResponse
    {
        public ulong Id { get; set; }
        public string Estado { get; set; } = null!;
        public string Sigla { get; set; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EAmbitoType Ambito { get; set; }
    }
}
