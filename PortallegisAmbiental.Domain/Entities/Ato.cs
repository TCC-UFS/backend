namespace PortalLegisAmbiental.Domain.Entities
{
    public class Ato
    {
        public ulong Id { get; private set; }
        public string Numero { get; private set; }
        public string Ementa { get; private set; }
        public DateTime DataPublicacao { get; private set; }
        public DateTime DataAto { get; private set; }
        public bool Disponivel { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public virtual ConteudoAto ConteudoAto { get; private set; } = null!;
        public ulong? CreatedById { get; private set; }
        public virtual Usuario? CreatedBy { get; private set; } = null!;
        public ulong JurisdicaoId { get; private set; }
        public virtual Jurisdicao Jurisdicao { get; private set; } = null!;
        public ulong TipoAtoId { get; private set; }
        public virtual TipoAto TipoAto { get; private set; } = null!;

        public Ato(
            string numero,
            string ementa,
            DateTime dataPublicacao,
            DateTime dataAto,
            bool disponivel)
        {
            Numero = numero;
            Ementa = ementa;
            DataPublicacao = dataPublicacao;
            DataAto = dataAto;
            Disponivel = disponivel;
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void UpdateNumber(string numero)
        {
            Numero = numero;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateEmenta(string ementa)
        {
            Ementa = ementa;
            UpdatedAt = DateTime.Now;
        }

        public void UpdatePublishDate(DateTime dataPublicacao)
        {
            DataPublicacao = dataPublicacao;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateDate(DateTime dataAto)
        {
            DataAto = dataAto;
            UpdatedAt = DateTime.Now;
        }

        public void Publish()
        {
            Disponivel = true;
            UpdatedAt = DateTime.Now;
        }

        public void Unpublish()
        {
            Disponivel = false;
            UpdatedAt = DateTime.Now;
        }

        public void SetCreatedById(ulong id)
        {
            CreatedById = id;
        }

        public void SetTipoAtoId(ulong id)
        {
            TipoAtoId = id;
        }

        public void SetJurisdicaoId(ulong id)
        {
            JurisdicaoId = id;
        }

        public void SetConteudoAto(ConteudoAto conteudoAto)
        {
            ConteudoAto = conteudoAto;
        }

        public void SetCreatedBy(Usuario usuario)
        {
            CreatedBy = usuario;
        }

        public void SetJurisdicao(Jurisdicao jurisdicao)
        {
            Jurisdicao = jurisdicao;
        }

        public void SetTipoAto(TipoAto tipoAto)
        {
            TipoAto = tipoAto;
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}
