using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.Dtos.Responses
{
    public class PermissaoResponse
    {
        public ulong Id { get; set; }
        public string Recurso { get; set; } = null!;
        public EScopeType Scope { get; set; }
    }
}
