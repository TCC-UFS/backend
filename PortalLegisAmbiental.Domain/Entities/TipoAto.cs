namespace PortalLegisAmbiental.Domain.Entities
{
    public class TipoAto
    {
        public ulong Id { get; private set; }
        public string Nome { get; private set; }
        public bool IsActive { get; private set; }

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
