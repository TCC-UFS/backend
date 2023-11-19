namespace PortalLegisAmbiental.Domain.Entities
{
    public class UsuarioGrupo
    {
        public ulong GrupoId { get; private set; }
        public ulong UsuarioId { get; private set; }
        public virtual Grupo Grupo { get; private set; } = null!;
        public virtual Usuario Usuario { get; private set; } = null!;

        public UsuarioGrupo(ulong grupoId, ulong usuarioId)
        {
            GrupoId = grupoId;
            UsuarioId = usuarioId;
        }

        public UsuarioGrupo()
        {
        }
    }
}
