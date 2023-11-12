namespace PortalLegisAmbiental.Domain.Entities
{
    public class Grupo
    {
        public ulong Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public virtual List<Usuario> Usuarios { get; set; } = new();
        public virtual List<UsuarioGrupo> UsuarioGrupos { get; set; } = new();
        public virtual List<Permissao> Permissoes { get; set; } = new();
        public virtual List<GrupoPermissao> GrupoPermissoes { get; set; } = new();

        public Grupo(string nome)
        {
            Nome = nome;
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void UpdateName(string? nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                Nome = nome;
                UpdatedAt = DateTime.Now;
            }
        }

        public void AddUser(Usuario usuario)
        {
            Usuarios.Add(usuario);
        }

        public bool RemoveUser(Usuario usuario)
        {
            return Usuarios.Remove(usuario);
        }

        public bool HasPermission(Permissao permissao)
        {
            return Permissoes.Find(perm => perm.Id.Equals(permissao.Id)) != null;
        }

        public void AddPermission(Permissao permissao)
        {
            Permissoes.Add(permissao);
        }

        public void RemovePermission(Permissao permissao)
        {
            var permissionIndex = Permissoes.FindIndex(perm => perm.Id.Equals(permissao.Id));
            if (permissionIndex > -1)
                Permissoes.RemoveAt(permissionIndex);
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
