using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UpdateUsuarioRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
