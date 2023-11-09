using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Permissao
    {
        public ulong Id { get; set; }
        public EScopeType Scope { get; set; }
        public string Recurso { get; set; }
        public bool IsActive { get; set; }

        public virtual List<Grupo> Grupos { get; set; }
    }
}
