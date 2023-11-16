using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UpdateJurisdicaoRequest
    {
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Ambito { get; set; }
        public string? Estado { get; set; }
        public string? Sigla { get; set; }
    }
}
