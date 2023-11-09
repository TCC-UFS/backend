namespace PortalLegisAmbiental.Domain.Entities
{
    public class Usuario
    {
        public ulong Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool IsActive { get; set; }

        public virtual List<Grupo> Grupos { get; set; } = new();
        public virtual List<UsuarioGrupo> UsuarioGrupos { get; set; } = new();
    }
}
