using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Jurisdicao
    {
        public ulong Id { get; set; }
        public EAmbitoType Ambito { get; set; }
        public string Sigla { get; set; }
        public string? Estado { get; set; }
        public bool IsActive { get; set; }
    }
}
