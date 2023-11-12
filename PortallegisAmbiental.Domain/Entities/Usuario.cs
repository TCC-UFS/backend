using Newtonsoft.Json;
using BC = BCrypt.Net.BCrypt;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Usuario
    {
        public ulong Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Senha { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; set; }

        public virtual List<Grupo> Grupos { get; set; } = new();
        public virtual List<UsuarioGrupo> UsuarioGrupos { get; set; } = new();

        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = BC.HashPassword(senha, BC.GenerateSalt(10));
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
            IsActive = true;
        }

        public void UpdateName(string? nome)
        {
            if (!string.IsNullOrWhiteSpace(nome))
            {
                Nome = nome;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdateEmail(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdatePassword(string? senha)
        {
            if (!string.IsNullOrEmpty(senha))
            {
                Senha = BC.HashPassword(senha, BC.GenerateSalt(10));
                UpdatedAt = DateTime.Now;
            }
        }

        public void AddGroup(Grupo grupo)
        {
            Grupos.Add(grupo);
        }

        public bool RemoveGroup(Grupo grupo)
        {
            return Grupos.Remove(grupo);
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
