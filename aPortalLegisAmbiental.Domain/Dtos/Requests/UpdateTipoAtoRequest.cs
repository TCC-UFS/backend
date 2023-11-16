using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UpdateTipoAtoRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Nome { get; set; }
    }
}
