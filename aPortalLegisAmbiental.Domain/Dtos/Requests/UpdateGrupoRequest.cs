using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UpdateGrupoRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Nome { get; set; }
    }
}
