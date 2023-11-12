using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UpdatePermissaoRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Recurso { get; set; }
        public string? Scope { get; set; }
    }
}
