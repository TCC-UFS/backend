namespace PortalLegisAmbiental.Domain.Entities
{
    public class Grupo
    {
        public ulong Id { get; set; }
        public string Nome { get; set; }
        public bool IsActive { get; set; }

        public virtual List<Usuario> Usuarios { get; set; } = new();
        public virtual List<UsuarioGrupo> UsuarioGrupos { get; set; } = new();
        public virtual List<Permissao> Permissoes { get; set; } = new();
        public virtual List<GrupoPermissao> GrupoPermissoes { get; set; } = new();
    }
}
