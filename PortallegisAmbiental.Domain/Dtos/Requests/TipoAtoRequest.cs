using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class TipoAtoRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string Nome { get; set; } = null!;
    }
}
