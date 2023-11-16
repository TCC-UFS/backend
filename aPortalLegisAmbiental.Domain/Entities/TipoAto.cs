namespace PortalLegisAmbiental.Domain.Entities
{
    public class TipoAto
    {
        public ulong Id { get; set; }
        public string Nome { get; set; }
        public bool IsActive { get; set; }

        public TipoAto(string nome)
        {
            Nome = nome;
            IsActive = true;
        }

        public void UpdateName(string? nome)
        {
            if (!string.IsNullOrEmpty(nome))
                Nome = nome;
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
