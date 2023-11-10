namespace PortalLegisAmbiental.Domain.Entities
{
    public class UsuarioGrupo
    {
        public ulong GrupoId { get; set; }
        public ulong UsuarioId { get; set; }
        public virtual Grupo Grupo { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;

        public UsuarioGrupo(ulong grupoId, ulong usuarioId)
        {
            GrupoId = grupoId;
            UsuarioId = usuarioId;
        }
    }
}
