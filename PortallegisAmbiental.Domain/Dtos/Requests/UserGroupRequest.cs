using System.Text.Json.Serialization;

namespace PortalLegisAmbiental.Domain.Dtos.Requests
{
    public class UserGroupRequest
    {
        [JsonIgnore]
        public ulong UserId { get; set; }
        public string Grupo { get; set; } = null!;
    }
}
