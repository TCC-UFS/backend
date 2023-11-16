namespace PortalLegisAmbiental.Domain.Entities
{
    public class GrupoPermissao
    {
        public ulong GrupoId { get; private set; }
        public ulong PermissaoId { get; private set; }
        public virtual Grupo Grupo { get; private set; } = null!;
        public virtual Permissao Permissao { get; private set; } = null!;

        public GrupoPermissao(ulong grupoId, ulong permissaoId)
        {
            GrupoId = grupoId;
            PermissaoId = permissaoId;
        }

        public GrupoPermissao()
        {
        }
    }
}
