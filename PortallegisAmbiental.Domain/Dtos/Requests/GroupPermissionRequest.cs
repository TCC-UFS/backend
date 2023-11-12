using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class GroupPermissionRequest
    {
        [JsonIgnore]
        public ulong GrupoId { get; set; }
        public string Recurso { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}
