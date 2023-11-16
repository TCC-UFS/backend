using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Domain.Entities
{
    public class Ato
    {
        public ulong Id { get; private set; }
        public string Numero { get; private set; } = null!;
        public string Ementa { get; private set; } = null!;
        public DateTime DataPublicacao { get; private set; }
        public DateTime DataAto { get; private set; }
        public string? CaminhoArquivo { get; private set; }
        public bool PossuiConteudo { get; private set; }
        public bool PossuiHtml { get; private set; }
        public bool Disponivel { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public ulong? CreatedById { get; private set; }
        public virtual Usuario? CreatedBy { get; private set; } = null!;
        public ulong JurisdicaoId { get; private set; }
        public virtual Jurisdicao Jurisdicao { get; private set; } = null!;
        public ulong TipoAtoId { get; private set; }
        public virtual TipoAto TipoAto { get; private set; } = null!;

        public Ato() { }

        public Ato(
            string numero,
            string ementa,
            DateTime dataPublicacao,
            DateTime dataAto,
            string? caminhoArquivo,
            bool possuiConteudo,
            bool possuiHtml,
            bool disponivel)
        {
            Numero = numero;
            Ementa = ementa;
            DataPublicacao = dataPublicacao;
            DataAto = dataAto;
            CaminhoArquivo = caminhoArquivo;
            PossuiConteudo = possuiConteudo;
            PossuiHtml = possuiHtml;
            Disponivel = disponivel;
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public Ato(AddAtoRequest request)
        {
            Numero = request.Numero;
            Ementa = request.Ementa;
            DataPublicacao = request.DataPublicacao;
            DataAto = request.DataAto;
            CaminhoArquivo = request.CaminhoArquivo;
            PossuiConteudo = !string.IsNullOrEmpty(request.Conteudo);
            PossuiHtml = !string.IsNullOrEmpty(request.Html);
            Disponivel = request.Disponivel;
            TipoAtoId = request.TipoAtoId;
            JurisdicaoId = request.JurisdicaoId;
            CreatedById = request.CreatedById;
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void UpdateNumber(string? numero)
        {
            if (!string.IsNullOrEmpty(numero))
            {
                Numero = numero;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdateEmenta(string? ementa)
        {
            if (!string.IsNullOrEmpty(ementa))
            {
                Ementa = ementa;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdatePublishDate(DateTime? dataPublicacao)
        {
            if (dataPublicacao.HasValue)
            {
                DataPublicacao = dataPublicacao.Value;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdateDate(DateTime? dataAto)
        {
            if (dataAto.HasValue)
            {
                DataAto = dataAto.Value;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdateFilePath(string? caminhoArquivo)
        {
            if (!string.IsNullOrEmpty(caminhoArquivo))
                CaminhoArquivo = caminhoArquivo;
        }

        public void HasContent()
        {
            PossuiConteudo = true;
        }

        public void HasHtml()
        {
            PossuiHtml = true;
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
