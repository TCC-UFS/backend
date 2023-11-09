namespace PortalLegisAmbiental.Domain.Entities
{
    public class GrupoPermissao
    {
        public ulong GrupoId { get; set; }
        public ulong PermissaoId { get; set; }
        public virtual Grupo Grupo { get; set; } = null!;
        public virtual Permissao Permissao { get; set; } = null!;
    }
}
